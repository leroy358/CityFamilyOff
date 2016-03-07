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
        //public ActionResult Login()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult Login(string userName, string password)
        //{
        //    Admins user = db.Admins.Where(item => item.AdminName == userName && item.Password == password && item.LoginState == 0).FirstOrDefault();
        //    if (user != null)
        //    {
        //        T_UserRole ur = db.T_UserRole.Where(o => o.UserID == user.Id).FirstOrDefault();
        //        T_RoleInfo rinfo = db.T_RoleInfo.Where(o => o.RoleID == ur.RoleID).FirstOrDefault();
        //        Session["userName"] = user.AdminName;
        //        Session["userId"] = user.Id;
        //        Session["companyId"] = user.CompanyID;
        //        Session["RoleId"] = rinfo.RoleID;
        //        Session.Timeout = 480;

        //        if (rinfo.RoleID == 5)
        //        {
        //            return Json("Success");
        //        }
        //        else
        //        {
        //            return Json("SuccessS");
        //        }
        //    }
        //    else
        //    {
        //        return Json("Error");
        //    }
        //}
        //public ActionResult Exit()
        //{
        //    Session.Clear();
        //    return RedirectToAction("Login");
        //}
        /// <summary>
        /// 登录，并获取用户数据
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>
        /// 若登录错误
        /// {
        ///     "state":"error"
        /// }
        /// 若登录成功，且用户身份为设计师
        /// {
        ///     "state":"success",
        ///     "data":{
        ///         "UserId":1,
        ///         "UserName":"张三",
        ///         "Password":"123456",
        ///         "CompanyId":"1",
        ///         "Identity":"设计师"
        ///     }
        /// }
        /// 若登录成功，且用户身份为设计师
        /// {
        ///     "state":"success",
        ///     "data":{
        ///         "UserId":1,
        ///         "UserName":"张三",
        ///         "Password":"123456",
        ///         "CompanyId":"1",
        ///         "Identity":"业务员"
        ///     }
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult Login(string userName, string password)
        {
            Admins user = db.Admins.Where(item => item.AdminName == userName && item.Password == password && item.LoginState == 0).FirstOrDefault();
            if (user != null)
            {
                T_UserRole ur = db.T_UserRole.Where(o => o.UserID == user.Id).FirstOrDefault();
                UserData userData = new UserData();
                userData.UserId = user.Id;
                userData.UserName = user.AdminName;
                userData.Password = user.Password;
                userData.CompanyId = user.CompanyID;
                if (ur.RoleID == 4)
                {
                    userData.Identity = "设计师";
                }
                else
                {
                    userData.Identity = "业务员";
                }
                return Json(new { state = "success", data = userData });
            }
            else
            {
                return Json(new { state = "error" });
            }
        }
    }
}