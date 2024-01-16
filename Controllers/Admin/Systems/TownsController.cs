using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using QLHN.Data;
using System.Security.Cryptography;
using QLHN.Helper;
using QLHN.Models.Systems;

namespace QLHN.Controllers.Admin.Systems
{
    public class TownsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TownsController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Route("Towns")]
        [HttpGet]
        public IActionResult Index(string MaHuyen)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                if (Funtions_Global.CheckPermission(HttpContext.Session, "systems.towns", "Index"))
                {
                    string MaTinh = _db.SystemInFo.OrderBy(t => t.MaTinh).First().MaTinh;
                    int count = _db.Districts.Count();
                    if (count > 0)
                    {
                        var districts = _db.Districts.Where(t => t.MaTinh == MaTinh);
                        if (string.IsNullOrEmpty(MaHuyen))
                        {
                            MaHuyen = districts.OrderBy(t => t.Id).Select(t => t.MaHuyen).First();
                        }
                        var model = _db.Towns.Where(t => t.MaHuyen == MaHuyen);

                        ViewData["Title"] = "Thông tin đơn vị ";
                        ViewData["MenuLv1"] = "menu_systems";
                        ViewData["MenuLv2"] = "menu_towns";
                        ViewData["MaHuyen"] = MaHuyen;
                        ViewData["Districts"] = districts;
                        return View("Views/Admin/Systems/Towns/Index.cshtml", model);

                    }
                    else
                    {
                        ViewData["Controller"] = "Districts";
                        ViewData["Action"] = "Index";
                        ViewData["Messages"] = "Bạn chưa nhập danh mục quận/ huyện nên không thể nhập danh mục xã phường";
                        ViewData["MenuLv1"] = "menu_systems";
                        ViewData["MenuLv2"] = "menu_towns";
                        return View("Views/Admin/Error/Page.cshtml");
                    }
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

        [Route("Towns/Store")]
        [HttpPost]
        public JsonResult Store(string MaHuyen, string MaXa, string TenXa, string MaTinh, double HeSoPhuCapKhuVuc)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                if (Funtions_Global.CheckPermission(HttpContext.Session, "systems.towns", "Create"))
                {
                    if (!string.IsNullOrEmpty(MaHuyen) && !string.IsNullOrEmpty(MaXa) && !string.IsNullOrEmpty(TenXa))
                    {
                        var check = _db.Towns.FirstOrDefault(p => p.MaXa == MaXa);
                        if (check == null)
                        {
                            var request = new Towns
                            {
                                MaHuyen = MaHuyen,
                                MaXa = MaXa,
                                TenXa = TenXa,
                                MaTinh = MaTinh,
                                /*HeSoPhuCapKhuVuc = HeSoPhuCapKhuVuc*/
                            };


                            _db.Towns.Add(request);
                            _db.SaveChanges();

                            var data = new { status = "success", message = "Cập nhật thành công!" };
                            return Json(data);
                        }
                        else
                        {
                            var data = new { status = "error", message = "Mã đơn vị: " + MaXa + " đã tồn tại " };
                            return Json(data);
                        }
                    }
                    else
                    {
                        var data = new { status = "error", message = "Mã đơn vị, tên đơn vị, username, password không được bỏ trống" };
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

        [Route("Towns/Edit")]
        [HttpPost]
        public JsonResult Edit(int Id)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                if (Funtions_Global.CheckPermission(HttpContext.Session, "systems.towns", "Edit"))
                {

                    var model = _db.Towns.FirstOrDefault(p => p.Id == Id);
                    if (model != null)
                    {
                        var districts = _db.Districts;
                        string result = "<div class='row' id='edit_thongtin'>";
                        result += "<div class='col-xl-6'>";
                        result += "<div class='form-group fv-plugins-icon-container'>";
                        result += "<label style='font-weight:bold'>Quận/huyện: </label>";
                        result += "<select id='mahuyen_edit' name='mahuyen_edit' class='form-control'>";
                        foreach (var item in districts)
                        {
                            if (item.MaHuyen == model.MaHuyen)
                            {
                                result += "<option value='" + item.MaHuyen + "' selected>" + item.TenHuyen + " </option>";
                            }
                            else
                            {
                                result += "<option value='" + item.MaHuyen + "'>" + item.TenHuyen + " </option>";
                            }
                        }
                        result += "</select>";
                        result += "</div>";
                        result += "</div>";
                        result += "<div class='col-xl-6'>";
                        result += "<div class='form-group fv-plugins-icon-container'>";
                        result += "<label style='font-weight:bold'>Mã đơn vị: </label>";
                        result += "<input type='text' id='maxa_edit' name='maxa_edit' class='form-control' value='" + model.MaXa + "' disabled/>";
                        result += "</div>";
                        result += "</div>";
                        result += "<div class='col-xl-6'>";
                        result += "<div class='form-group fv-plugins-icon-container'>";
                        result += "<label style='font-weight:bold'>Tên đơn vị: </label>";
                        result += "<input type='text' id='tenxa_edit' name='tenxa_edit' class='form-control' value='" + model.TenXa + "'/>";
                        result += "</div>";
                        result += "</div>";
                        /*result += "<div class='col-xl-6'>";
                        result += "<div class='form-group fv-plugins-icon-container'>";
                        result += "<label style='font-weight:bold'>Phụ cấp khu vực (%)</label>";
                        result += "<input type='number' step='1' id='hesophucapkhuvuc_edit' name='hesophucapkhuvuc_edit' class='form-control' value='" + model.HeSoPhuCapKhuVuc + "'/>";
                        result += "</div>";
                        result += "</div>";*/
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

        [Route("Towns/Update")]
        [HttpPost]
        public JsonResult Update(int Id, string MaHuyen, string MaXa, string TenXa, double HeSoPhuCapKhuVuc)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                if (Funtions_Global.CheckPermission(HttpContext.Session, "systems.towns", "Edit"))
                {
                    if (!string.IsNullOrEmpty(TenXa))
                    {
                        var model = _db.Towns.FirstOrDefault(t => t.Id == Id);
                        model.MaHuyen = MaHuyen;
                        model.MaXa = MaXa;
                        model.TenXa = TenXa;
                        /*model.HeSoPhuCapKhuVuc = HeSoPhuCapKhuVuc;*/
                        _db.Towns.Update(model);
                        _db.SaveChanges();

                        var data = new { status = "success", message = "Cập nhật thành công!" };
                        return Json(data);
                    }
                    else
                    {
                        var data = new { status = "error", message = "Mã đơn vị và tên đơn vị không được bỏ trống" };
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

        [Route("Towns/Delete")]
        [HttpPost]
        public IActionResult Delete(int id_delete)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                if (Funtions_Global.CheckPermission(HttpContext.Session, "systems.towns", "Delete"))
                {
                    var model = _db.Towns.FirstOrDefault(p => p.Id == id_delete);
                    _db.Towns.Remove(model);
                    _db.SaveChanges();
                    return RedirectToAction("Index", "Towns");
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
    }
}
