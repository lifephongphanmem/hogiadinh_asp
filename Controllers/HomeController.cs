using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QLHN.Data;
using QLHN.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using QLHN.Helper;
using QLHN.Models.Systems;
using System.Text.RegularExpressions;
using QLHN.ViewModels.Api;
using Newtonsoft.Json;
using System.Security.Policy;

namespace QLHN.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                string MaTinh = _db.SystemInFo.OrderBy(t => t.Id).First().MaTinh;
                ViewData["TenTinh"] = _db.Cities.FirstOrDefault(t => t.MaTinh == MaTinh).TenTinh;
                ViewData["BanQuyen"] = "Sở LĐTB&XH " + _db.Cities.FirstOrDefault(t => t.MaTinh == MaTinh).TenTinh;
                ViewData["Title"] = "Trang chủ";
                ViewData["MenuLv1"] = "menu_home";
                return View("Views/Admin/Home/Dashboard/Index.cshtml");
            }
        }
        [Route("ChangePassword")]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                ViewData["Title"] = "Thay đổi mật khẩu";
                return View("Views/Admin/Home/ChangePassword.cshtml");
            }
            else
            {
                return View("Views/Admin/Error/SessionOut.cshtml");
            }
        }
        [Route("ChangePassword")]
        [HttpPost]
        public IActionResult ChangePassword(string current_password, string new_password, string verify_password)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                if (!string.IsNullOrEmpty(current_password) && !string.IsNullOrEmpty(new_password) && !string.IsNullOrEmpty(verify_password))
                {
                    string md5_password = "";
                    using (MD5 md5Hash = MD5.Create())
                    {
                        string change = Funtions_Global.GetMd5Hash(md5Hash, current_password);
                        md5_password = change;
                    }
                    if (md5_password == Funtions_Global.GetSsAdmin(HttpContext.Session, "Password"))
                    {
                        if (new_password == verify_password)
                        {
                            Regex regex = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,20}$");

                            if (regex.IsMatch(verify_password))
                            {
                                string new_md5_password = "";
                                using (MD5 md5Hash = MD5.Create())
                                {
                                    string new_change = Funtions_Global.GetMd5Hash(md5Hash, verify_password);
                                    new_md5_password = new_change;
                                }
                                var model = _db.Users.FirstOrDefault(u => u.Username == Funtions_Global.GetSsAdmin(HttpContext.Session, "Username"));
                                model.Password = new_md5_password;
                                _db.SaveChanges();

                                return RedirectToAction("LogOut", "Login");
                            }
                            else
                            {
                                string error = "Mật khẩu từ phải có ít nhất 4 ký tự và không quá 20 ký tự, bao gồm ít nhất 1 chữ cái viết hoa, 1 chữ cái viết thường và một chữ số ";
                                ViewData["Title"] = "Thay đổi mật khẩu";
                                ViewData["current_password"] = current_password;
                                ViewData["new_password"] = new_password;
                                ViewData["verify_password"] = verify_password;
                                ModelState.AddModelError("error", error);
                                return View("Views/Admin/Home/ChangePassword.cshtml");
                            }
                        }
                        else
                        {
                            ViewData["Title"] = "Thay đổi mật khẩu";
                            ModelState.AddModelError("error", "Mật khẩu mới và mật khẩu xác thực không trùng nhau");
                            ViewData["current_password"] = current_password;
                            ViewData["new_password"] = new_password;
                            ViewData["verify_password"] = verify_password;
                            return View("Views/Admin/Home/ChangePassword.cshtml");
                        }
                    }
                    else
                    {
                        ViewData["Title"] = "Thay đổi mật khẩu";
                        ModelState.AddModelError("error", "Mật khẩu hiện tại không đúng");
                        ViewData["current_password"] = current_password;
                        ViewData["new_password"] = new_password;
                        ViewData["verify_password"] = verify_password;
                        return View("Views/Admin/Home/ChangePassword.cshtml");
                    }
                }
                else
                {
                    ViewData["Title"] = "Thay đổi mật khẩu";
                    ModelState.AddModelError("error", "Thông tin không được bỏ trống");
                    ViewData["current_password"] = current_password;
                    ViewData["new_password"] = new_password;
                    ViewData["verify_password"] = verify_password;
                    return View("Views/Admin/Home/ChangePassword.cshtml");
                }
            }
            else
            {
                return View("Views/Admin/Error/SessionOut.cshtml");
            }
        }

        [Route("ThongTinDonVi/Edit")]
        [HttpGet]
        public IActionResult Edit()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                var model = _db.Users.FirstOrDefault(t => t.Username == Funtions_Global.GetSsAdmin(HttpContext.Session, "Username"));
                ViewData["Title"] = "Quản lý thông tin đơn vị";
                return View("Views/Admin/Home/ThongTinDonVi.cshtml", model);
            }
            else
            {
                return View("Views/Admin/Error/SessionOut.cshtml");
            }
        }
        [HttpPost]
        public IActionResult Update(Users requests)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                var model = _db.Users.FirstOrDefault(t => t.Id == requests.Id);
                model.TenDonViChuQuan = requests.TenDonViChuQuan;
                model.TenDonVi = requests.TenDonVi;
                model.TenBoPhan = requests.TenBoPhan;
                model.MaDonViSDNS = requests.MaDonViSDNS;
                model.DiaDanh = requests.DiaDanh;
                _db.Users.Update(model);
                _db.SaveChanges();

                return RedirectToAction("LogOut", "Login");
            }
            else
            {
                return View("Views/Admin/Error/SessionOut.cshtml");
            }
        }
        [Route("SyncUser/Create")]
        public IActionResult SyncUserCreate()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                var url = _db.SystemInFo.FirstOrDefault().Url;
                //ViewData["url"] = url;
                ViewData["Title"] = "Quản lý thông tin đồng bộ";
                return View("Views/Admin/Home/SyncUser.cshtml");
            }
            else
            {
                return View("Views/Admin/Error/SessionOut.cshtml");
            }
        }
        [Route("CheckSync")]
        [HttpPost]
        public async Task<IActionResult> CheckSync()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                var user = Funtions_Global.GetSsAdmin(HttpContext.Session, "Username");
                var url = _db.SystemInFo.FirstOrDefault().Url;
                var parameters = new Dictionary<string, string>
                        {
                            {"UserNCC",user},
                         };
                var data_Sso = await Funtions_Global.GetDataClient(url, "api/NguoiCoCong/GetUserSynchronization", parameters);
                var datas = JsonConvert.DeserializeObject<VMDataReturnCheckSync>(data_Sso);
                string result = "";
                if (data_Sso == "error" || datas == null)
                {
                    result += "<div class='modal-body' style='font-weight:bold; color:red'><h2>" + "Không thể kết nối đến phần mềm tổng hợp xin hãy kiểm tra lại !!!" + "</h2></div>";
                }
                else
                {
                    var model = datas.Data;
                    if (datas.Status)
                    {
                        result += "<div class='modal-body'>";
                        result += "<div class='row'>";
                        result += "<div class='col-xl-12'>";
                        result += "<div class='form-group fv-plugins-icon-container'>";
                        result += "<label style='font-weight:bold; color:darkgreen'>" + datas.Message + "!!!</label>";
                        result += "</div>";
                        result += "</div>";
                        result += "<div class='col-xl-12'>";
                        result += "<div class='form-group fv-plugins-icon-container'>";
                        result += "<label style='font-weight:bold; color:blue'>Tên tài khoản tổng hợp đã đồng bộ: </label>";
                        result += "<span > " + model.Name + "</span>";
                        result += "</div>";
                        result += "</div>";
                        result += "<div class='col-xl-12'>";
                        result += "<div class='form-group fv-plugins-icon-container'>";
                        result += "<label style='font-weight:bold; color:blue'>Tài khoản tổng hợp đã đồng bộ: </label>";
                        result += "<span > " + model.Username + "</span>";
                        result += "</div>";
                        result += "</div>";
                        result += "</div>";
                        result += "</div>";
                    }
                    else
                    {
                        result += "<div class='modal-body' style='font-weight:bold; color:red'><h2>" + datas.Message + "</h2></div>";
                    }
                }
                var data = new { status = "success", message = result };
                return Json(data);
            }
            else
            {
                var data = new { status = "logout", message = "Kết thúc phiên làm việc xin hãy đăng nhập lại để tiếp tục làm việc" };
                return Json(data);
            }
        }

        [Route("SyncUser")]
        [HttpPost]
        public async Task<IActionResult> SyncUser(VMSyncUser Request)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "");
                    ViewData["Title"] = "Quản lý thông tin đồng bộ";
                    return View("Views/Admin/Home/SyncUser.cshtml", Request);
                }
                else
                {
                    var username = Funtions_Global.GetSsAdmin(HttpContext.Session, "Username");
                    string md5_passwordNCC = "";
                    string md5_passwordSso = "";
                    using (MD5 md5Hash = MD5.Create())
                    {
                        string changeNCC = Funtions_Global.GetMd5Hash(md5Hash, Request.PasswordNCC);
                        string changeSso = Funtions_Global.GetMd5Hash(md5Hash, Request.Password);
                        md5_passwordNCC = changeNCC;
                        md5_passwordSso = changeSso;
                    }
                    var check = _db.Users.FirstOrDefault(x => x.Username == username && x.Password == md5_passwordNCC);
                    if (check != null)
                    {
                        var url = _db.SystemInFo.FirstOrDefault().Url;
                        // call API tổng hợp
                        var parameters = new Dictionary<string, string>
                        {
                            {"Username",Request.Username},
                            {"Password",md5_passwordSso },
                            {"UserNCC",username },
                         };
                        string data_Sso = await Funtions_Global.GetDataClient(url, "api/UserSynchronization", parameters);
                        var data = JsonConvert.DeserializeObject<VMDataReturn>(data_Sso);
                        if (data_Sso == "error" || data == null)
                        {
                            ModelState.AddModelError("", "Không thể kết nối đến phần mềm tổng hợp xin hãy kiểm tra lại !!!");
                            ViewData["Title"] = "Quản lý thông tin đồng bộ";
                            return View("Views/Admin/Home/SyncUser.cshtml", Request);
                        }
                        else
                        {
                            if (data.Status)
                            {
                                ViewData["Messages"] = "Đồng bộ tài khoản thành công";
                                ViewData["Controller"] = "Home";
                                ViewData["Action"] = "Index";
                                return View("Views/Admin/Error/PageSuccess.cshtml");
                            }
                            else
                            {
                                if (data.Message == "Username không tồn tại")
                                {
                                    ModelState.AddModelError("Username", "Tài khoản tổng hợp không tồn tại");
                                    ViewData["Title"] = "Quản lý thông tin đồng bộ";
                                    return View("Views/Admin/Home/SyncUser.cshtml", Request);
                                }
                                else
                                {
                                    ModelState.AddModelError("Password", "Mật khẩu tài khoản tổng hợp không đúng");
                                    ViewData["Title"] = "Quản lý thông tin đồng bộ";
                                    return View("Views/Admin/Home/SyncUser.cshtml", Request);
                                }

                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("PasswordNCC", "Mật khẩu hiện tại không đúng");
                        ViewData["Title"] = "Quản lý thông tin đồng bộ";
                        return View("Views/Admin/Home/SyncUser.cshtml", Request);
                    }
                }
            }
            else
            {
                return View("Views/Admin/Error/SessionOut.cshtml");
            }
        }

        [Route("ThemeSettings")]
        [HttpGet]
        public IActionResult ThemeSettings()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
                {
                    var model = _db.Users.FirstOrDefault(t => t.Username == Funtions_Global.GetSsAdmin(HttpContext.Session, "Username"));
                    ViewData["Title"] = "Quản lý giao diện hiển thị";
                    return View("Views/Admin/Home/ThemeSettings.cshtml", model);
                }
                else
                {
                    return View("Views/Admin/Error/SessionOut.cshtml");
                }
            }
            else
            {
                return View("Views/Admin/Error/SessionOut.cshtml");
            }
        }

        [HttpPost]
        public IActionResult UpdateTheme(Users requests)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                var model = _db.Users.FirstOrDefault(t => t.Id == requests.Id);
                model.Menu = requests.Menu;
                model.Theme = requests.Theme;
                model.Content = requests.Content;
                _db.Users.Update(model);
                _db.SaveChanges();

                return RedirectToAction("LogOut", "Login");
            }
            else
            {
                return View("Views/Admin/Error/SessionOut.cshtml");
            }
        }

        [Route("DanhSachTaiKhoanTapHuan")]
        [HttpGet]
        public IActionResult DanhSachTaiKhoanTapHuan(string Level, string MaHuyen)
        {
            string MaTinh = _db.SystemInFo.OrderBy(t => t.Id).First().MaTinh;
            Level = string.IsNullOrEmpty(Level) ? "all" : Level;
            MaHuyen = string.IsNullOrEmpty(MaHuyen) ? "all" : MaHuyen;
            var model = _db.Users.Where(t => t.MaTinh == MaTinh && !t.Sadmin);
            if (Level != "all")
            {
                model = model.Where(t => t.Level == Level);
                if (Level == "X" && MaHuyen != "all")
                {
                    model = model.Where(t => t.MaHuyen == MaHuyen);
                }
            }
            ViewData["MaHuyen"] = MaHuyen;
            ViewData["Level"] = Level;
            ViewData["Districts"] = _db.Districts.Where(t => t.MaTinh == MaTinh);
            ViewData["Title"] = "Danh sách tài khoản tập huấn";
            return View("Views/Admin/Systems/DanhSachTapHuan/Index.cshtml", model);
        }




    }
}
