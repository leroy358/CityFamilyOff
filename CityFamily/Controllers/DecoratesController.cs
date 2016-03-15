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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="decorateId"></param>
        /// <returns>
        /// {
        ///     "data":{
        ///         "DecorateId":35,
        ///         "DecoratePics":"/Images/data/201507/b480a31d-3f82-4903-85c5-11f0f2a7a2aa0.jpg /Images/data/201507/28c599a0-d4b2-4c82-870f-28877c2455e41.jpg ",
        ///         "Decorate360":null
        ///     }
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult GetDecorateDetail(int decorateId)
        {
            Decorate decorate = db.Decorate.Find(decorateId);
            DecorateDetailData decorateDetail = new DecorateDetailData();
            decorateDetail.DecorateId = decorateId;
            decorateDetail.DecoratePics = decorate.DecoratePics;
            decorateDetail.Decorate360 = decorate.Decorate360;
            return Json(new { data = decorateDetail }, JsonRequestBehavior.AllowGet);
        }
	}
}