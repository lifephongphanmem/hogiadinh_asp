using Microsoft.AspNetCore.Mvc;
using QLHN.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QLHN.Helper;
using Microsoft.AspNetCore.Http;
using QLHN.ViewModels.Systems;
using QLHN.Models.Systems;

namespace QLHN.Controllers.Admin.Systems
{
    public class GroupPermissionController : Controller
    {
        private readonly ApplicationDbContext _db;

        public GroupPermissionController(ApplicationDbContext db)
        {
            _db = db;
        }
        [Route("GroupPermission")]
        [HttpGet]
        public IActionResult Index()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                if (Funtions_Global.CheckPermission(HttpContext.Session, "systems.group_permission", "Index"))
                {
                    var model = _db.GroupPermissions;
                    ViewData["Title"] = "Thông tin quyền truy cập";
                    ViewData["MenuLv1"] = "menu_systems";
                    ViewData["MenuLv2"] = "menu_grop_per";
                    return View("Views/Admin/Systems/GroupPermission/Index.cshtml", model);
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

        [Route("GroupPermission/Create")]
        [HttpPost]
        public IActionResult Create(string PhanLoai)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                if (Funtions_Global.CheckPermission(HttpContext.Session, "systems.group_permission", "Create"))
                {
                    var per = Funtions_Global.GetRolesList();
                    var model_del = _db.Permissions.Where(t => t.Status == "Disable");
                    _db.Permissions.RemoveRange(model_del);
                    _db.SaveChanges();
                    string KeyLink = "Per_" + DateTime.Now.ToString("yymmssfff");
                    this.SetPermission(KeyLink, PhanLoai);
                    var dmroles = _db.RolesAction.Where(t => t.Roles.Contains("manages"));
                    var pms = from pm in _db.Permissions
                              join roles in _db.RolesAction on pm.Roles equals roles.Roles
                              where pm.Username == KeyLink
                              select new Permissions
                              {
                                  Id = pm.Id,
                                  Index = pm.Index,
                                  Create = pm.Create,
                                  Edit = pm.Edit,
                                  Delete = pm.Delete,
                                  Approve = pm.Approve,
                                  MoTa = roles.MoTa,
                                  Roles = pm.Roles,

                              };
                    var model = new VMGroupPermissionCe
                    {
                        KeyLink = KeyLink,
                        Permissions = pms.OrderBy(t=>t.Id).ToList(),
                    };
                    ViewData["dmroles"] = dmroles;
                    ViewData["Title"] = "Thông tin quyền truy cập";
                    ViewData["MenuLv1"] = "menu_systems";
                    ViewData["MenuLv2"] = "menu_grop_per";
                    return View("Views/Admin/Systems/GroupPermission/Create.cshtml", model);
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

        [Route("GroupPermission/Store")]
        [HttpPost]
        public IActionResult Store(VMGroupPermissionCe requests)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                if (Funtions_Global.CheckPermission(HttpContext.Session, "systems.group_permission", "Create"))
                {
                    this.GroupPermission(requests);
                    this.Permission(requests.KeyLink);
                    return RedirectToAction("Index", "GroupPermission");
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

        [Route("GroupPermission/Edit")]
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                if (Funtions_Global.CheckPermission(HttpContext.Session, "systems.group_permission", "Edit"))
                {
                    var model_group = _db.GroupPermissions.FirstOrDefault(t => t.Id == Id);
                    var dmroles = _db.RolesAction.Where(t => t.Roles.Contains("manages"));
                    var model_per = from pm in _db.Permissions.Where(t=>t.Username == model_group.KeyLink)
                                    join roles in _db.RolesAction on pm.Roles equals roles.Roles
                                    select new Permissions
                                    {
                                        Id = pm.Id,
                                        Index = pm.Index,
                                        Create = pm.Create,
                                        Edit = pm.Edit,
                                        Delete = pm.Delete,
                                        Approve = pm.Approve,
                                        MoTa = roles.MoTa,
                                        Roles = pm.Roles,

                                    };

                    var model = new VMGroupPermissionCe
                    {
                        KeyLink = model_group.KeyLink,
                        GroupName = model_group.GroupName,
                        MoTa = model_group.MoTa,
                        Id = model_group.Id,
                        Permissions = model_per.ToList()
                    };
                    ViewData["dmroles"] = dmroles;
                    ViewData["Title"] = "Thông tin quyền truy cập";
                    ViewData["MenuLv1"] = "menu_systems";
                    ViewData["MenuLv2"] = "menu_grop_per";
                    return View("Views/Admin/Systems/GroupPermission/Edit.cshtml", model);
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

        [Route("GroupPermission/Update")]
        [HttpPost]
        public IActionResult Update(VMGroupPermissionCe requests)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                if (Funtions_Global.CheckPermission(HttpContext.Session, "systems.group_permission", "Create"))
                {
                    this.GroupPermission(requests);
                    this.Permission(requests.KeyLink);
                    ViewData["Title"] = "Thông tin quyền truy cập";
                    ViewData["MenuLv1"] = "menu_systems";
                    ViewData["MenuLv2"] = "menu_grop_per";
                    return RedirectToAction("Index", "GroupPermission");
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

        [Route("GroupPermission/Delete")]
        [HttpPost]
        public IActionResult Delete(int id_delete)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SsAdmin")))
            {
                if (Funtions_Global.CheckPermission(HttpContext.Session, "systems.group_permission", "Delete"))
                {
                    var model_del = _db.GroupPermissions.FirstOrDefault(t => t.Id == id_delete);
                    var model_per_del = _db.Permissions.Where(t => t.Username == model_del.KeyLink);
                    _db.Permissions.RemoveRange(model_per_del);
                    _db.GroupPermissions.Remove(model_del);
                    _db.SaveChanges();
                    ViewData["Title"] = "Thông tin quyền truy cập";
                    ViewData["MenuLv1"] = "menu_systems";
                    ViewData["MenuLv2"] = "menu_grop_per";
                    return RedirectToAction("Index", "GroupPermission");
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

        public void GroupPermission(VMGroupPermissionCe requests)
        {
            if (requests.Id == 0)
            {
                var model = new GroupPermission
                {
                    KeyLink = requests.KeyLink,
                    GroupName = requests.GroupName,
                    MoTa = requests.MoTa
                };
                _db.GroupPermissions.Add(model);
                _db.SaveChanges();
            }
            else
            {
                var model = _db.GroupPermissions.FirstOrDefault(t => t.Id == requests.Id);
                model.MoTa = requests.MoTa;
                model.GroupName = requests.GroupName;
                _db.GroupPermissions.Update(model);
                _db.SaveChanges();
            }
        }

        public void Permission(string Username)
        {
            var model = _db.Permissions.Where(t => t.Username == Username);
            foreach (var item in model)
            {
                item.Status = "Enable";
            }
            _db.Permissions.UpdateRange(model);
            _db.SaveChanges();
        }

        public void SetPermission(string KeyLink, string PhanLoai)
        {
            IEnumerable<RolesAction> model_roles = _db.RolesAction;
            if(PhanLoai == "HT")
            {
                model_roles = model_roles.Where(t => !t.Roles.Contains("manages"));
            }
            else
            {
                model_roles = model_roles.Where(t => t.Roles.Contains("manages"));
            }
            List<Permissions> new_lists = new List<Permissions>();
            foreach (var roles in model_roles)
            {
                new_lists.Add(new Permissions { Username = KeyLink, Roles = roles.Roles, Index = true, Status = "Disable" });
            }
            _db.Permissions.AddRange(new_lists);
            _db.SaveChanges();
        }

    }
}
