using CityFamily.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityFamily.Controllers
{
    public class UserController : Controller
    {
        private CityFamilyDbContext db = new CityFamilyDbContext();
        [HttpPost]
        public ActionResult ChangePsw(int id, string oldPsw, string newPsw)
        {
            Admins user = db.Admins.Find(id);
            if (oldPsw.Trim() != user.Password)
            {
                return Json(new { data = "Error" });
            }
            else
            {
                user.Password = newPsw;
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { data = "Success" });
            }
        }
    }
}