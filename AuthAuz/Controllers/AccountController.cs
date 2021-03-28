using AuthAuz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AuthAuz.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(tblUsers obj)
        {
            using (var _context = new AppDbContext())
            {
                bool isValid = _context.TblUsers.Any(u => u.UserName == obj.UserName && u.UserPassword == obj.UserPassword);
                if (isValid)
                {
                    FormsAuthentication.SetAuthCookie(obj.UserName, false);
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid User Name or Password");
                    return View();
                }
            }

        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(tblUsers obj)
        {
            using (var _context = new AppDbContext())
            {
                bool isExist = !_context.TblUsers.Any(u => u.UserName == obj.UserName);
                if (isExist)
                {
                    _context.TblUsers.Add(obj);
                    _context.SaveChanges();
                    int count = _context.TblUsers.Count();
                    if (count == 1)
                    {

                        return RedirectToAction("CreateRole", "Admin");
                    }
                    else
                    {
                        return View("Login");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User Already Exist");
                    return View();
                }

            }

        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}