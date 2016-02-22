using CityFamily.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityFamily.Controllers
{
    public class HomeController : Controller
    {
        private CityFamilyDbContext db = new CityFamilyDbContext();
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string userName, string password)
        {
            Admins user = db.Admins.Where(item => item.AdminName == userName && item.Password == password && item.LoginState == 0).FirstOrDefault();
            if (user != null)
            {
                T_UserRole ur = db.T_UserRole.Where(o => o.UserID == user.Id).FirstOrDefault();
                T_RoleInfo rinfo = db.T_RoleInfo.Where(o => o.RoleID == ur.RoleID).FirstOrDefault();
                Session["userName"] = user.AdminName;
                Session["userId"] = user.Id;
                Session["companyId"] = user.CompanyID;
                Session["RoleId"] = rinfo.RoleID;
                Session.Timeout = 480;

                if (rinfo.RoleID == 5)
                {
                    return Json("Success");
                }
                else
                {
                    return Json("SuccessS");
                }
            }
            else
            {
                return Json("Error");
            }
        }
        public ActionResult Exit()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}