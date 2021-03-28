using AuthAuz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AuthAuz.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult CreateRole()
        {
            using (var _context = new AppDbContext())
            {
                List<tblUsers> userList = _context.TblUsers.ToList();
                ViewBag.Users = new SelectList(userList, "ID", "UserName");
                return View();
            }
        }
        [HttpPost]
        public ActionResult CreateRole(tblRoles obj)
        {
            using (var _context = new AppDbContext())
            {
                _context.TblRoles.Add(obj);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        public ActionResult Index()
        {
            using (var _context = new AppDbContext())
            {
                var UserRoleList = (from user in _context.TblUsers join role in _context.TblRoles on user.ID equals role.UserID select new { UserID = user.ID, RoleID = role.ID, UserName = user.UserName, RoleName = role.RoleName }).ToList();
                List<ViewModelUser> list = new List<ViewModelUser>();
                foreach (var item in UserRoleList)
                {
                    ViewModelUser obj = new ViewModelUser();
                    obj.RoleID = item.RoleID;
                    obj.RoleName = item.RoleName;
                    obj.ID = item.UserID;
                    obj.UserName = item.UserName;
                    list.Add(obj);
                }
                return View(list);
            }
        }
        public ActionResult Edit(int id)
        {
            using (var _context = new AppDbContext())
            {
                tblRoles role = _context.TblRoles.Find(id);
                List<tblUsers> userList = _context.TblUsers.ToList();
                ViewBag.Users = new SelectList(userList, "ID", "UserName");
                return View(role);
            }
        }
        [HttpPost]
        public ActionResult Edit(tblRoles obj)
        {
            using (var _context = new AppDbContext())
            {
                bool IsExist = !_context.TblRoles.Any(u => u.RoleName == obj.RoleName && u.UserID == obj.UserID);
                if (IsExist)
                {
                    tblRoles roleObj = _context.TblRoles.Find(obj.ID);
                    roleObj.RoleName = obj.RoleName;
                    roleObj.UserID = obj.UserID;
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    tblRoles role = _context.TblRoles.Find(obj.ID);
                    List<tblUsers> userList = _context.TblUsers.ToList();
                    ViewBag.Users = new SelectList(userList, "ID", "UserName");
                    ModelState.AddModelError("", "Role Already Exist");
                    return View();
                }

            }

        }
        public ActionResult Delete(int id)
        {
            using (var _context = new AppDbContext())
            {
                tblRoles role = _context.TblRoles.Find(id);
                role.TblUser = _context.TblUsers.Find(role.ID);
                return View(role);
            }
        }
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            using (var _context = new AppDbContext())
            {
                tblRoles role = _context.TblRoles.Find(id);
                if (role != null)
                {
                    _context.TblRoles.Remove(role);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    role.TblUser = _context.TblUsers.Find(role.ID);
                    return View(role);
                }

            }

        }
        public ActionResult Details(int id)
        {
            using (var _context = new AppDbContext())
            {
                tblRoles role = _context.TblRoles.Find(id);
                List<tblUsers> userList = _context.TblUsers.ToList();
                ViewBag.Users = new SelectList(userList, "ID", "UserName");
                return View(role);
            }
        }
    }
}