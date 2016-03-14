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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spotId"></param>
        /// <returns>
        /// {
        ///     "data":{
        ///         "SpotId":35,
        ///         "SpotPics":"/Images/data/201507/b480a31d-3f82-4903-85c5-11f0f2a7a2aa0.jpg /Images/data/201507/28c599a0-d4b2-4c82-870f-28877c2455e41.jpg "
        ///     }
        /// }
        /// </returns>
        public ActionResult GetSpotDetail(int spotId)
        {
            SpotPics spot = db.SpotPics.Find(spotId);
            SpotPicsDetailData spotDetail = new SpotPicsDetailData();
            spotDetail.SpotId = spotId;
            spotDetail.SpotPics = spot.SpotDetails;
            return Json(new { data = spotDetail }, JsonRequestBehavior.AllowGet);
        }
    }
}