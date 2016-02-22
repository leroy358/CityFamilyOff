using CityFamily.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityFamily.Areas.Admin.Controllers
{
    public class ConsoleController : Controller
    {
        private CityFamilyDbContext db = new CityFamilyDbContext();
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            Admins admin = db.Admins.Where(item => item.AdminName == username && item.Password == password && item.LoginState == 1).FirstOrDefault();
            if (admin != null)
            {
                Session["admin"] = admin.AdminName;
                Session["adminId"] = admin.Id;
                Session["cid"] = admin.CompanyID;
                Session.Timeout = 2 * 60;
                return Json("Success");
            }
            else
            {
                return Json("Error");
            }
        }
        public ActionResult Main()
        {
            if (Session["admin"] != null)
            {
                int adminId = Session["adminId"] != null ? int.Parse(Session["adminId"].ToString()) : 0;
                List<int> userrole = db.T_UserRole.Where(o => o.UserID == adminId).Select(s => s.RoleID).ToList();
                List<int> authinfo = db.T_AuthInfo.Where(o => o.AuthMaster == "Role" && userrole.Contains(o.AuthMasterValue)).Select(o => o.AuthAccessValue).ToList();
                List<T_MenuInfo> menuinfo = db.T_MenuInfo.Where(o => (authinfo.Contains(o.ParentMenuID))).ToList();
                return View(menuinfo);
            }
            else
            {
                return RedirectToAction("Login");
            }

        }
        public ActionResult Exit()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}