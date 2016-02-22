using CityFamily.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityFamily.Controllers
{
    public class SpotPicsController : Controller
    {
        private CityFamilyDbContext db = new CityFamilyDbContext();
        public ActionResult SpotShow(int id)
        {
            if (Session["userName"] != null)
            {
                var spotPic = db.SpotPics.Find(id);
                return View(spotPic);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
	}
}