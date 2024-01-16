using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using QLHN.Data;
using QLHN.Helper;

namespace QLHN.Controllers
{
    public class AjaxController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AjaxController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Route("Ajax/GetTowns")]
        [HttpPost]
        public JsonResult GetSeSelectTowns(string MaHuyen, string KeySelect)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                if (string.IsNullOrEmpty(MaHuyen))
                {
                    string result = "";
                    result = "<select class='form-control' id='" + KeySelect + "' name='" + KeySelect + "'> ";
                    result += "<option value='all'>---Chọn đơn vị---</option>";
                    result += "</select>";
                    var data = new { status = "success", message = result };
                    return Json(data);
                }
                else
                {
                    var towns = _db.Towns.Where(t => t.MaHuyen == MaHuyen);
                    string result = "";
                    result = "<select class='form-control' id='" + KeySelect + "' name='" + KeySelect + "'> ";
                    result += "<option value='all'>---Chọn đơn vị---</option>";
                    foreach (var item in towns)
                    {
                        result += "<option value='" + item.MaXa + "'>" + item.TenXa + "</option>";
                    }
                    result += "</select>";
                    var data = new { status = "success", message = result };
                    return Json(data);
                }
            }
            else
            {
                var data = new { status = "error", message = "Bạn kêt thúc phiên đăng nhập! Đăng nhập lại để tiếp tục công việc" };
                return Json(data);
            }
        }

        [Route("Ajax/GetTownsWithSession")]
        [HttpPost]
        public JsonResult GetTownsWithSession(string MaHuyen, string KeySelect)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                string MaTinh = _db.SystemInFo.OrderBy(t => t.Id).First().MaTinh;
                var towns = _db.Towns.Where(t => t.MaTinh == MaTinh && t.MaHuyen == MaHuyen);
                string result = "";
                if (Funtions_Global.GetSsAdmin(HttpContext.Session,"Level") == "X")
                {
                    string MaXa = Funtions_Global.GetSsAdmin(HttpContext.Session, "MaXa");
                    towns = towns.Where(t => t.MaXa == MaXa);
                    
                    result = "<select class='form-control' id='" + KeySelect + "' name='" + KeySelect + "'> ";
                    foreach (var item in towns)
                    {
                        result += "<option value='" + item.MaXa + "'>" + item.TenXa + "</option>";
                    }
                    result += "</select>";                   
                }
                else
                {                   
                    result = "<select class='form-control' id='" + KeySelect + "' name='" + KeySelect + "'> ";
                    result += "<option value='all'>---Chọn đơn vị---</option>";
                    foreach (var item in towns)
                    {
                        result += "<option value='" + item.MaXa + "'>" + item.TenXa + "</option>";
                    }
                    result += "</select>";                   
                   
                }
                var data = new { status = "success", message = result };
                return Json(data);

            }
            else
            {
                var data = new { status = "error", message = "Bạn kêt thúc phiên đăng nhập! Đăng nhập lại để tiếp tục công việc" };
                return Json(data);
            }
        }

        [Route("Ajax/GetTownsNoAll")]
        [HttpPost]
        public JsonResult GetTownsNoAll(string MaHuyen, string KeySelect)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                if (string.IsNullOrEmpty(MaHuyen))
                {
                    string result = "";
                    result = "<select class='form-control' id='" + KeySelect + "' name='" + KeySelect + "'> ";
                    result += "<option value=''>---Chọn đơn vị---</option>";
                    result += "</select>";
                    var data = new { status = "success", message = result };
                    return Json(data);
                }
                else
                {
                    var towns = _db.Towns.Where(t => t.MaHuyen == MaHuyen);
                    string result = "";
                    result = "<select class='form-control' id='" + KeySelect + "' name='" + KeySelect + "'> ";
                    foreach (var item in towns)
                    {
                        result += "<option value='" + item.MaXa + "'>" + item.TenXa + "</option>";
                    }
                    result += "</select>";
                    var data = new { status = "success", message = result };
                    return Json(data);
                }
            }
            else
            {
                var data = new { status = "error", message = "Bạn kêt thúc phiên đăng nhập! Đăng nhập lại để tiếp tục công việc" };
                return Json(data);
            }
        }

        [Route("Ajax/GetSelectMaHuyenMaXa")]
        [HttpPost]
        public JsonResult GetSelectMaHuyenMaXa(string MaTinh, string Id_MaHuyen, string Id_MaXa, string FunChange)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                var districts = _db.Districts.Where(t => t.MaTinh == MaTinh);

                string result = "";
                result = "<select class='form-control' id='" + Id_MaHuyen + "' name='" + Id_MaHuyen + "' onchange='"+ FunChange + "'> ";
                result += "<option value=''>---Chọn đơn vị---</option>";
                foreach (var huyen in districts)
                {
                    result += "<option value='" + huyen.MaHuyen + "'>" + huyen.TenHuyen + "</option>";
                }
                result += "</select>";
                                
                string result1 = "";
                result1 = "<select class='form-control' id='" + Id_MaXa + "' name='" + Id_MaXa + "'> ";
                result1 += "<option value=''>---Chọn đơn vị---</option>";               
                result1 += "</select>";

                var data = new { status = "success", message = result , message1 = result1};
                return Json(data);

            }
            else
            {
                var data = new { status = "error", message = "Bạn kêt thúc phiên đăng nhập! Đăng nhập lại để tiếp tục công việc" };
                return Json(data);
            }
        }

        [Route("Ajax/GetSelectMaXa")]
        [HttpPost]
        public JsonResult GetSelectMaXa(string MaTinh, string MaHuyen, string Id_MaXa)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                var towns = _db.Towns.Where(t => t.MaTinh == MaTinh && t.MaHuyen == MaHuyen);

                string result = "";
                result = "<select class='form-control' id='" + Id_MaXa + "' name='" + Id_MaXa + "'> ";
                result += "<option value=''>---Chọn đơn vị---</option>";
                foreach (var xa in towns)
                {
                    result += "<option value='" + xa.MaXa + "'>" + xa.TenXa + "</option>";
                }
                result += "</select>";
                
                var data = new { status = "success", message = result };
                return Json(data);

            }
            else
            {
                var data = new { status = "error", message = "Bạn kêt thúc phiên đăng nhập! Đăng nhập lại để tiếp tục công việc" };
                return Json(data);
            }
        }



    }
}
