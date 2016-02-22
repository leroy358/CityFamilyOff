using CityFamily.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityFamily.Controllers
{
    public class DecoratesController : Controller
    {
        private CityFamilyDbContext db = new CityFamilyDbContext();
        public ActionResult DecorateDetails(int id)
        {
            if (Session["userName"] != null)
            {
                var decorate = db.Decorate.Find(id);
                return View(decorate);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }        
	}
}