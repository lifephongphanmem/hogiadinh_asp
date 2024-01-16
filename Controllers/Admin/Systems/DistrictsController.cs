using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Http;
using QLHN.Data;
using System.Security.Cryptography;
using QLHN.Helper;
using QLHN.Models.Systems;

namespace QLHN.Controllers.Admin.Systems
{
    public class DistrictsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public DistrictsController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Route("Districts")]
        [HttpGet]
        public IActionResult Index()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                if (Funtions_Global.CheckPermission(HttpContext.Session, "systems.districts", "Index"))
                {
                    string MaTinh = _db.SystemInFo.OrderBy(t=>t.MaTinh).First().MaTinh;
                    var model = _db.Districts.Where(t => t.MaTinh == MaTinh); ;
                    ViewData["MaTinh"] = MaTinh;
                    ViewData["Title"] = "Thông tin đơn vị ";
                    ViewData["MenuLv1"] = "menu_systems";
                    ViewData["MenuLv2"] = "menu_districts";
                    return View("Views/Admin/Systems/Districts/Index.cshtml", model);
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

        [Route("Districts/Store")]
        [HttpPost]
        public JsonResult Store(string MaHuyen, string TenHuyen, string MaTinh)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                if (Funtions_Global.CheckPermission(HttpContext.Session, "systems.thongtindonvi", "Create"))
                {
                    if (!string.IsNullOrEmpty(MaHuyen) && !string.IsNullOrEmpty(TenHuyen))
                    {
                        var check = _db.Districts.FirstOrDefault(p => p.MaHuyen == MaHuyen);
                        if (check == null)
                        {
                            var request = new Districts
                            {
                                MaHuyen = MaHuyen,
                                TenHuyen = TenHuyen,
                                MaTinh = MaTinh
                            };                               
                            _db.Districts.Add(request);
                            _db.SaveChanges();

                            var data = new { status = "success", message = "Cập nhật thành công!" };
                            return Json(data);                       
                        }
                        else
                        {
                            var data = new { status = "error", message = "Mã đơn vị: " + MaHuyen + " đã tồn tại " };
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

        [Route("Districts/Edit")]
        [HttpPost]
        public JsonResult Edit(int Id)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                if (Funtions_Global.CheckPermission(HttpContext.Session, "systems.thongtindonvi", "Edit"))
                {

                    var model = _db.Districts.FirstOrDefault(p => p.Id == Id);
                    if (model != null)
                    {
                        string result = "<div class='row' id='edit_thongtin'>";
                        result += "<div class='col-xl-6'>";
                        result += "<div class='form-group fv-plugins-icon-container'>";
                        result += "<label>Mã đơn vị: </label>";
                        result += "<input type='text' id='mahuyen_edit' name='mahuyen_edit' class='form-control' value='" + model.MaHuyen + "' disabled/>";
                        result += "</div>";
                        result += "</div>";
                        result += "<div class='col-xl-6'>";
                        result += "<div class='form-group fv-plugins-icon-container'>";
                        result += "<label>Tên đơn vị: </label>";
                        result += "<input type='text' id='tenhuyen_edit' name='tenhuyen_edit' class='form-control' value='" + model.TenHuyen + "'/>";
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

        [Route("Districts/Update")]
        [HttpPost]
        public JsonResult Update(int Id, string MaHuyen, string TenHuyen)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                if (Funtions_Global.CheckPermission(HttpContext.Session, "systems.thongtindonvi", "Edit"))
                {
                    if (!string.IsNullOrEmpty(TenHuyen))
                    {
                        var model = _db.Districts.FirstOrDefault(t => t.Id == Id);
                        model.TenHuyen = TenHuyen;
                        model.MaHuyen = MaHuyen;                       
                        _db.Districts.Update(model);
                        _db.SaveChanges();

                        var data = new { status = "success", message = "Cập nhật thành công!"};
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

        [Route("Districts/Delete")]
        [HttpPost]
        public IActionResult Delete(int id_delete)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                if (Funtions_Global.CheckPermission(HttpContext.Session, "systems.districts", "Delete"))
                {
                    var model = _db.Districts.FirstOrDefault(p => p.Id == id_delete);
                    _db.Districts.Remove(model);
                    _db.SaveChanges();                   
                    return RedirectToAction("Index", "Districts");                   
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
