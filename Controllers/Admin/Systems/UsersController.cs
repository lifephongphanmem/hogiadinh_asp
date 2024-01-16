using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using QLHN.Data;
using QLHN.Models.Systems;
using System.Security.Cryptography;
using QLHN.Helper;
using QLHN.ViewModels.Systems;

namespace NCC.Controllers.Admin.Systems
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;

        public UsersController(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            _hostEnvironment = hostEnvironment;
        }

        [Route("Users")]
        [HttpGet]
        public IActionResult Index(string Level)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                if (Funtions_Global.CheckPermission(HttpContext.Session, "systems.users", "Index"))
                {
                    if (string.IsNullOrEmpty(Level))
                    {
                        Level = "all";
                    }
                    var model = _db.Users.Where(u => u.Status != "Register" && u.Sadmin != true);
                    if (Level != "all")
                    {
                        model = model.Where(u => u.Level == Level);
                    }
                    string MaTinh = _db.SystemInFo.OrderBy(t => t.Id).First().MaTinh;
                    var districts = _db.Districts.Where(t => t.MaTinh == MaTinh);
                    var mahuyen_first = districts.OrderBy(t => t.Id).Select(t => t.MaHuyen).First();
                    var towns = _db.Towns.Where(t => t.MaHuyen == mahuyen_first);
                    ViewData["GroupPer"] = _db.GroupPermissions;
                    ViewData["Districts"] = districts;
                    ViewData["Towns"] = towns;
                    ViewData["Title"] = "Thông tin tài khoản";
                    ViewData["selected_level"] = Level;
                    ViewData["MenuLv1"] = "menu_systems";
                    ViewData["MenuLv2"] = "menu_users";
                    return View("Views/Admin/Systems/Users/Index.cshtml", model);

                }
                else
                {
                    ViewData["Messages"] = "Bạn không có quyền truy cập vào chức năng này!";
                    return View("Views/Admin/Error/Page.cshtml");
                }
            }
            else
            {
                return View("Views/Admin/Error/SessionOut.cshtml");
            }
        }


        [Route("Users/Store")]
        [HttpPost]
        public JsonResult Store(string MaHuyen, string MaXa, string Name, string Username, string Password, string Group)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                if (Funtions_Global.CheckPermission(HttpContext.Session, "systems.users", "Create"))
                {
                    if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
                    {
                        var check_username = _db.Users.FirstOrDefault(u => u.Username == Username);
                        if (check_username == null)
                        {
                            string md5_password = "";
                            using (MD5 md5Hash = MD5.Create())
                            {
                                string change = Funtions_Global.GetMd5Hash(md5Hash, Password);
                                md5_password = change;
                            }
                            string level = "";
                            if (MaHuyen == "all" && MaXa == "all")
                            {
                                level = "T";
                            }
                            else if (MaHuyen != "all" && MaXa == "all")
                            {
                                level = "H";
                            }
                            else
                            {
                                level = "X";
                            }
                            var request_user = new Users
                            {
                                MaTinh = _db.SystemInFo.OrderBy(t => t.MaTinh).First().MaTinh,
                                Username = Username,
                                Password = md5_password,
                                Name = Name,
                                MaHuyen = MaHuyen,
                                MaXa = MaXa,
                                Status = "Active",
                                Level = level,
                                Sadmin = false,
                                Avatar = "default-user.png",
                                Group = Group
                            };
                            _db.Users.Add(request_user);
                            _db.SaveChanges();

                            var data = new { status = "success", message = "Cập nhật thành công!" };
                            return Json(data);
                        }
                        else
                        {
                            var data = new { status = "error", message = "Username: " + Username + " đã tồn tại " };
                            return Json(data);
                        }
                    }
                    else
                    {
                        var data = new { status = "error", message = "Tên đơn vị, username, password không được bỏ trống" };
                        return Json(data);
                    }
                }
                else
                {
                    var data = new { status = "error", message = "Bạn không có quyền thực hiện chức năng này!!!" };
                    return Json(data);
                }
            }
            else
            {
                var data = new { status = "error", message = "Bạn kêt thúc phiên đăng nhập! Đăng nhập lại để tiếp tục công việc" };
                return Json(data);
            }
        }

        [Route("Users/Edit")]
        [HttpPost]
        public JsonResult Edit(int Id)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                if (Funtions_Global.CheckPermission(HttpContext.Session, "systems.users", "Edit"))
                {
                    string matinh = _db.SystemInFo.OrderBy(t => t.MaTinh).First().MaTinh;
                    string tentinh = _db.Cities.FirstOrDefault(t => t.MaTinh == matinh).TenTinh;
                    var model = _db.Users.FirstOrDefault(p => p.Id == Id);
                    if (model != null)
                    {
                        var districts = _db.Districts.Where(t => t.MaTinh == matinh);
                        var towns = _db.Towns.Where(t => t.MaHuyen == model.MaHuyen);
                        string result = "<div class='row' id='edit_thongtin'>";
                        if (model.Level == "T")
                        {
                            result += "<div class='col-xl-4'>";
                            result += "<div class='form-group fv-plugins-icon-container'>";
                            result += "<label>Tỉnh/Thành phố:</label>";
                            result += "<label style='font-weight:bold;color:blue'>" + tentinh + "</label>";
                            result += "</div>";
                            result += "</div>";
                        }
                        else if (model.Level == "H")
                        {
                            result += "<div class='col-xl-4'>";
                            result += "<div class='form-group fv-plugins-icon-container'>";
                            result += "<label>Tỉnh/Thành phố:</label>";
                            result += "<label style='font-weight:bold;color:blue'>" + tentinh + "</label>";
                            result += "</div>";
                            result += "</div>";
                            result += "<div class='col-xl-4'>";
                            result += "<div class='form-group fv-plugins-icon-container'>";
                            result += "<label>Quận/Huyện:</label>";
                            result += "<label style='font-weight:bold;color:blue'>" + _db.Districts.FirstOrDefault(t => t.MaHuyen == model.MaHuyen).TenHuyen + "</label>";
                            result += "</div>";
                            result += "</div>";
                        }
                        else
                        {
                            result += "<div class='col-xl-4'>";
                            result += "<div class='form-group fv-plugins-icon-container'>";
                            result += "<label>Tỉnh/Thành phố:</label>";
                            result += "<label style='font-weight:bold;color:blue'>" + tentinh + "</label>";
                            result += "</div>";
                            result += "</div>";
                            result += "<div class='col-xl-4'>";
                            result += "<div class='form-group fv-plugins-icon-container'>";
                            result += "<label>Quận/Huyện:</label>";
                            result += "<label style='font-weight:bold;color:blue'>" + districts.FirstOrDefault(t => t.MaHuyen == model.MaHuyen).TenHuyen + "</label>";
                            result += "</div>";
                            result += "</div>";
                            result += "<div class='col-xl-4'>";
                            result += "<div class='form-group fv-plugins-icon-container'>";
                            result += "<label>Quận/Huyện:</label>";
                            result += "<label style='font-weight:bold;color:blue'>";
                            result += _db.Towns.FirstOrDefault(t => t.MaHuyen == model.MaHuyen && t.MaXa == model.MaXa).TenXa + "</label>";
                            result += "</div>";
                            result += "</div>";
                        }

                        result += "<div class='col-xl-12'>";
                        result += "<div class='form-group fv-plugins-icon-container'>";
                        result += "<label>Tên tài khoản: </label>";
                        result += "<input type='text' id='name_edit' name='name_edit' class='form-control' value='" + model.Name + "'/>";
                        result += "</div>";
                        result += "</div>";
                        result += "<div class='col-xl-6'>";
                        result += "<div class='form-group fv-plugins-icon-container'>";
                        result += "<label>Usersname</label>";
                        result += "<span type='text' id='username_edit' name='username_edit' class='form-control'>" + model.Username + "</span>";
                        result += "</div>";
                        result += "</div>";
                        result += "<div class='col-xl-6'>";
                        result += "<div class='form-group fv-plugins-icon-container'>";
                        result += "<label>Mật khẩu: </label>";
                        result += "<input type='text' id='password_edit' name='password_edit' class='form-control'/>";
                        result += "</div>";
                        result += "</div>";

                        result += "<div class='col-xl-6'>";
                        result += "<div class='form-group fv-plugins-icon-container'>";
                        result += "<label>Trạng thái: </label>";
                        result += "<select id='status_edit' name='status_edit' class='form-control'>";
                        result += "<option value='Active'" + (model.Status == "Active" ? "selected" : "") + ">Active</option>";
                        result += "<option value='Lock'" + (model.Status == "Lock" ? "selected" : "") + ">Lock</option>";
                        result += "</select>";
                        result += "</div>";
                        result += "</div>";


                        result += "<div class='col-xl-6'>";
                        result += "<div class='form-group fv-plugins-icon-container'>";
                        result += "<label>Nhóm quyền </label>";
                        result += "<select id='group_edit' name='group_edit' class='form-control'>";

                        foreach (var item in _db.GroupPermissions)
                        {
                            if (model.Group == item.KeyLink)
                            {
                                result += "<option value='" + item.KeyLink + "' selected>" + item.GroupName + "</option>";
                            }
                            else
                            {
                                result += "<option value='" + item.KeyLink + "'>" + item.GroupName + "</option>";
                            }
                        }
                        result += "</select>";
                        result += "</div>";
                        result += "</div>";
                        result += "<input hidden type='text' id='id_edit' name='id_edit' value='" + model.Id + "'/>";
                        result += "</div>";

                        var data = new { status = "success", message = result };
                        return Json(data);
                    }
                    else
                    {
                        var data = new { status = "error", message = "Không tìm thấy thông tin cần chỉnh sửa!!!" };
                        return Json(data);
                    }
                }
                else
                {
                    var data = new { status = "error", message = "Bạn không có quyền thực hiện chức năng này!!!" };
                    return Json(data);
                }
            }
            else
            {
                var data = new { status = "error", message = "Bạn kêt thúc phiên đăng nhập! Đăng nhập lại để tiếp tục công việc" };
                return Json(data);
            }
        }

        [Route("Users/Update")]
        [HttpPost]
        public JsonResult Update(string Username, string Name, string Password, string Group, int Id, string Status)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                if (Funtions_Global.CheckPermission(HttpContext.Session, "systems.users", "Create"))
                {
                    if (!string.IsNullOrEmpty(Name))
                    {
                        int check = _db.Users.Where(t => t.Username == Username && t.Id != Id).Count();
                        if (check == 0)
                        {
                            var model = _db.Users.FirstOrDefault(u => u.Id == Id);
                            if (!string.IsNullOrEmpty(Password))
                            {
                                using (MD5 md5Hash = MD5.Create())
                                {
                                    string change = Funtions_Global.GetMd5Hash(md5Hash, Password);
                                    model.Password = change;
                                }
                            }
                            if (Group != "K")
                            {
                                var del_percustom = _db.Permissions.Where(t => t.Username == Username);
                                _db.Permissions.RemoveRange(del_percustom);
                                _db.SaveChanges();
                            }
                            model.Name = Name;
                            model.Group = Group;
                            model.Status = Status;
                            model.CountLogin = 0;
                            _db.Users.Update(model);
                            _db.SaveChanges();

                            var data = new { status = "success", message = "Cập nhật thành công!" };
                            return Json(data);
                        }
                        else
                        {
                            var data = new { status = "error", message = "Username đã tồn tại!!!" };
                            return Json(data);
                        }
                    }
                    else
                    {
                        var data = new { status = "error", message = "Tên đơn vị không được bỏ trống" };
                        return Json(data);
                    }
                }
                else
                {
                    var data = new { status = "error", message = "Bạn không có quyền thực hiện chức năng này!!!" };
                    return Json(data);
                }
            }
            else
            {
                var data = new { status = "error", message = "Bạn kêt thúc phiên đăng nhập! Đăng nhập lại để tiếp tục công việc" };
                return Json(data);
            }
        }

        [Route("Users/Delete")]
        [HttpPost]
        public IActionResult Delete(int id_delete)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                if (Funtions_Global.CheckPermission(HttpContext.Session, "systems.users", "Delete"))
                {
                    var model = _db.Users.FirstOrDefault(p => p.Id == id_delete);
                    _db.Users.Remove(model);
                    _db.SaveChanges();
                    return RedirectToAction("Index", "Users");
                }
                else
                {
                    ViewData["Messages"] = "Bạn không có quyền truy cập vào chức năng này!";
                    return View("Views/Admin/Error/Page.cshtml");
                }
            }
            else
            {
                return View("Views/Admin/Error/SessionOut.cshtml");
            }
        }

        [Route("Users/Permission")]
        [HttpGet]
        public IActionResult Permissions(string Username)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                if (Funtions_Global.CheckPermission(HttpContext.Session, "systems.users", "Index"))
                {
                    var dmroles = _db.RolesAction.Where(t => t.Roles.Contains("manages"));
                    var model = (from ps in _db.Permissions
                                 join role in _db.RolesAction on ps.Roles equals role.Roles
                                 where ps.Username == Username
                                 select new Permissions
                                 {
                                     Id = ps.Id,
                                     Index = ps.Index,
                                     Create = ps.Create,
                                     Edit = ps.Edit,
                                     Delete = ps.Delete,
                                     Approve = ps.Approve,
                                     MoTa = role.MoTa,
                                     Roles = ps.Roles,
                                 }).ToList();
                    ViewData["DSPermissions"] = dmroles;
                    ViewData["Username"] = Username;
                    ViewData["Title"] = "Thông tin quyền truy cập";
                    ViewData["MenuLv1"] = "menu_systems";
                    ViewData["MenuLv2"] = "menu_users";
                    return View("Views/Admin/Systems/Users/PermissionCustom.cshtml", model);
                }
                else
                {
                    ViewData["Messages"] = "Bạn không có quyền truy cập vào chức năng này!";
                    return View("Views/Admin/Error/Page.cshtml");
                }
            }
            else
            {
                return View("Views/Admin/Error/SessionOut.cshtml");
            }
        }
        [HttpPost]
        public IActionResult ResetPassword(int id_reset)
        {
            var model = _db.Users.FirstOrDefault(x => x.Id == id_reset);
            using (MD5 md5Hash = MD5.Create())
            {
                string change = Funtions_Global.GetMd5Hash(md5Hash, "Life@2012!");
                model.Password = change;
            }
            _db.SaveChanges();
            return RedirectToAction("Index", "Users");
        }
    }
}
