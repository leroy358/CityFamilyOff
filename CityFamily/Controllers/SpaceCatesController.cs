using CityFamily.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityFamily.Controllers
{
    public class SpaceCatesController : Controller
    {
        private CityFamilyDbContext db = new CityFamilyDbContext();
        public ActionResult SpaceShow(int cate)
        {
            var pics = db.SpaceCate.Where(item => item.Category == cate).FirstOrDefault();
            return View(pics);
        }
	}
}