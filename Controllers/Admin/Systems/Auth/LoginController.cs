using Microsoft.AspNetCore.Mvc;
using QLHN.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using QLHN.Helper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using QLHN.Models.Systems;
using QLHN.ViewModels.Api;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;
using System.Runtime.Intrinsics.X86;

namespace QLHN.Controllers.Admin.Systems.Auth
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _db;
        public LoginController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Route("Login")]
        [HttpGet]
        public IActionResult Login(string Username)
        {
            string MaTinh = _db.SystemInFo.OrderBy(t => t.Id).First().MaTinh;
            bool sso = _db.SystemInFo.OrderBy(t => t.Id).First().SSO;
            ViewBag.status = sso;
            ViewData["Title"] = "Login";
            ViewData["Username"] = Username;           
            ViewData["BanQuyen"] = "Phần mềm Cuộc Sống " + _db.Cities.FirstOrDefault(t => t.MaTinh == MaTinh).TenTinh;
            return View("Views/Admin/Systems/Auth/Login.cshtml");
        }

        [Route("SignIn")]
        [HttpPost]
        public IActionResult SignIn(string username, string password)
        {
            if (username != null && password != null)
            {
                var model = _db.Users.FirstOrDefault(u => u.Username == username);

                if (model != null)
                {
                    if (model.Status == "Lock")
                    {
                        ModelState.AddModelError("error", "Tài khoản đã bị khóa. Liên hệ với quản trị hệ thống !!!");
                        bool sso = _db.SystemInFo.OrderBy(t => t.Id).First().SSO;
                        ViewBag.status = sso;
                        ViewData["username"] = username;
                        ViewData["password"] = password;
                        return View("Views/Admin/Systems/Auth/Login.cshtml");
                    }
                    else
                    {
                        string md5_password = "";
                        using (MD5 md5Hash = MD5.Create())
                        {
                            string change = Funtions_Global.GetMd5Hash(md5Hash, password);
                            md5_password = change;
                        }
                        //return Ok(md5_password);
                        if (md5_password == model.Password)
                        {
                            model.CountLogin = 0;
                            _db.Users.Update(model);
                            _db.SaveChanges();
                            HttpContext.Session.SetString("SsAdmin", JsonConvert.SerializeObject(model));
                            if (model.Group == "K")
                            {
                                var permissions = _db.Permissions.Where(p => p.Username == username);
                                HttpContext.Session.SetString("Permission", JsonConvert.SerializeObject(permissions));
                            }
                            else
                            {
                                var permissions = _db.Permissions.Where(p => p.Username == model.Group);
                                HttpContext.Session.SetString("Permission", JsonConvert.SerializeObject(permissions));
                            }
                           
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            int locklogin = _db.SystemInFo.OrderBy(t => t.Id).First().LoginLock;
                            int count = model.CountLogin + 1;
                            if (count == locklogin)
                            {
                                model.Status = "Lock";
                            }
                            else
                            {
                                model.CountLogin = count;
                            }
                            _db.Users.Update(model);
                            _db.SaveChanges();
                            ViewData["username"] = username;
                            ModelState.AddModelError("error", "Mật khẩu truy cập không đúng!!!(" + count + "/" + locklogin + ")");
                            bool sso = _db.SystemInFo.OrderBy(t => t.Id).First().SSO;
                            ViewBag.status = sso;
                            ViewData["Title"] = "Login";
                            return View("Views/Admin/Systems/Auth/Login.cshtml");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("error", "Tài khoản/Mật khẩu truy cập không đúng !!!");
                    bool sso = _db.SystemInFo.OrderBy(t => t.Id).First().SSO;
                    ViewBag.status = sso;
                    ViewData["Title"] = "Login";
                    return View("Views/Admin/Systems/Auth/Login.cshtml");
                }
            }
            else
            {
                ModelState.AddModelError("error", "Tài khoản/Mật khẩu truy cập không được để trống !!!");
                bool sso = _db.SystemInFo.OrderBy(t => t.Id).First().SSO;
                ViewBag.status = sso;
                ViewData["Title"] = "Login";
                return View("Views/Admin/Systems/Auth/Login.cshtml");
            }
        }

        [Route("SignInSso")]
        [HttpPost]
        public async Task<IActionResult> SignInSso(string Username)
        {            
            var url = _db.SystemInFo.FirstOrDefault().Url;
            // call API tổng hợp
            var parameters = new Dictionary<string, string>
                        {
                            {"username",Username},                            
                         };
            string data_Sso = await Funtions_Global.GetDataClient(url, "api/NguoiCoCong/CheckExpiryTimeUser", parameters);
            var data = JsonConvert.DeserializeObject<VMDataReturn>(data_Sso);
            if (data.Status)
            {
                var model = _db.Users.FirstOrDefault(u => u.Username == data.Data);
                if (model != null)
                {
                    if (model.Status == "Lock")
                    {
                        ModelState.AddModelError("error", "Tài khoản đã bị khóa. Liên hệ với quản trị hệ thống !!!");
                        bool sso = _db.SystemInFo.OrderBy(t => t.Id).First().SSO;
                        ViewBag.status = sso;
                        ViewData["username"] = data.Data;                        
                        return View("Views/Admin/Systems/Auth/Login.cshtml");
                    }
                    else
                    {           
                            HttpContext.Session.SetString("SsAdmin", JsonConvert.SerializeObject(model));
                            if (model.Group == "K")
                            {
                                var permissions = _db.Permissions.Where(p => p.Username == data.Data);
                                HttpContext.Session.SetString("Permission", JsonConvert.SerializeObject(permissions));
                            }
                            else
                            {
                                var permissions = _db.Permissions.Where(p => p.Username == model.Group);
                                HttpContext.Session.SetString("Permission", JsonConvert.SerializeObject(permissions));
                            }

                            return RedirectToAction("Index", "Home");                        
                    }
                }
                else
                {
                    ModelState.AddModelError("error", "Tài khoản truy cập không đúng !!!");
                    bool sso = _db.SystemInFo.OrderBy(t => t.Id).First().SSO;
                    ViewBag.status = sso;
                    ViewData["Title"] = "Login";
                    return View("Views/Admin/Systems/Auth/Login.cshtml");
                }                
            }
            else
            {
                string MaTinh = _db.SystemInFo.OrderBy(t => t.Id).First().MaTinh;
                ModelState.AddModelError("error",data.Message);
                ViewBag.status = true;
                ViewData["Title"] = "Login";
                ViewData["Username"] = Username;
                ViewData["BanQuyen"] = "Sở LĐTB&XH " + _db.Cities.FirstOrDefault(t => t.MaTinh == MaTinh).TenTinh;
                return View("Views/Admin/Systems/Auth/Login.cshtml");
            }
        }

        [Route("Logout")]
        [HttpGet]
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("Permission");
            HttpContext.Session.Remove("SsAdmin");
            return RedirectToAction("Login", "Login");
        }
    }
}
