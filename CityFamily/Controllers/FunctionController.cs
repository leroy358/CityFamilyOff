using CityFamily.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityFamily.Controllers
{
    public class FunctionController : Controller
    {
        private CityFamilyDbContext db = new CityFamilyDbContext();
        public ActionResult Questionnaire()
        {
            if (Session["userName"] != null)
            {
                var space = db.SpaceCate;
                return View(space);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        //[HttpPost]
        //public void Submit(string dataJson, string guestName, string guestPhone)
        //{
        //    if (Session["userName"] != null)
        //    {
        //        //var stream = HttpContext.Request.InputStream;
        //        //string dataJson = new StreamReader(stream).ReadToEnd(); //json 字符串在此
        //        FuncResult funcResult = new FuncResult();
        //        funcResult.Result = dataJson;

        //        //////////////////////////////////////////////////////////////
        //        funcResult.GuestName = guestName;
        //        funcResult.GuestPhone = guestPhone;
        //        //////////////////////////////////////////////////////////////

        //        funcResult.UserId = (int)Session["userId"];
        //        funcResult.IsOver = false;
        //        funcResult.IsLead = 1;
        //        db.FuncResult.Add(funcResult);
        //        db.SaveChanges();
        //    }
        //}
        [HttpPost]
        public void Submit()
        {
            if (Session["userName"] != null)
            {
                var stream = HttpContext.Request.InputStream;
                string dataJson = new StreamReader(stream).ReadToEnd(); //json 字符串在此
                FuncResult funcResult = new FuncResult();
                funcResult.Result = dataJson;
                funcResult.UserId = (int)Session["userId"];
                funcResult.IsOver = false;
                funcResult.IsLead = 1;
                db.FuncResult.Add(funcResult);
                db.SaveChanges();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// {
        ///     "material":"http://home.vrcan.com/Home/Iv/21730",
        ///     "menTing":"http://119.188.113.104:8081/SpaceCate/MenTing/MenTing.html",
        ///     "keTing":"http://119.188.113.104:8081/SpaceCate/KeTing/KeTing.html",
        ///     "canTing":"http://119.188.113.104:8081/SpaceCate/CanTing/CanTing.html",
        ///     "zhuWo":"http://119.188.113.104:8081/SpaceCate/ZhuRenFang/ZhuRenFang.html",
        ///     "laoRen":"http://119.188.113.104:8081/SpaceCate/LaoRenFang/LaoRenFang.html",
        ///     "erTong":"http://119.188.113.104:8081/SpaceCate/ErTongFang/ErTongFang.html",
        ///     "chuFang":"http://119.188.113.104:8081/SpaceCate/ChuFang/ChuFang.html",
        ///     "weiSheng":"http://119.188.113.104:8081/SpaceCate/WeiShengJian/WeiShengJian.html",
        ///     "chuWu":"http://119.188.113.104:8081/Spacecate/YiMaoJian/YiMaoJian.html"
        ///     "duoGongNeng":"http://119.188.113.104:8081/Spacecate/DuoGongNeng/DuoGongNeng.html"
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult GetSpaceLink()
        {
            string material = db.SpaceCate.Where(item => item.Category == 23).FirstOrDefault().SpacePics;
            string menTing = db.SpaceCate.Where(item => item.Category == 1).FirstOrDefault().SpacePics;
            string keTing = db.SpaceCate.Where(item => item.Category == 10).FirstOrDefault().SpacePics;
            string canTing = db.SpaceCate.Where(item => item.Category == 11).FirstOrDefault().SpacePics;
            string zhuWo = db.SpaceCate.Where(item => item.Category == 3).FirstOrDefault().SpacePics;
            string laoRen = db.SpaceCate.Where(item => item.Category == 4).FirstOrDefault().SpacePics;
            string erTong = db.SpaceCate.Where(item => item.Category == 5).FirstOrDefault().SpacePics;
            string chuFang = db.SpaceCate.Where(item => item.Category == 6).FirstOrDefault().SpacePics;
            string weiSheng = db.SpaceCate.Where(item => item.Category == 7).FirstOrDefault().SpacePics;
            string chuWu = db.SpaceCate.Where(item => item.Category == 8).FirstOrDefault().SpacePics;
            string duoGongNeng = db.SpaceCate.Where(item => item.Category == 9).FirstOrDefault().SpacePics;

            return Json(new { material = material, menTing = menTing, keTing = keTing, canTing = canTing, zhuWo = zhuWo, laoRen = laoRen, erTong = erTong, chuFang = chuFang, weiSheng = weiSheng, chuWu = chuWu, duoGongNeng = duoGongNeng }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult UploadResult(int userId,string dataJson)
        {
            //var stream = HttpContext.Request.InputStream;
            //string dataJson = new StreamReader(stream).ReadToEnd(); //json 字符串在此
            //JObject jo = (JObject)JsonConvert.DeserializeObject(dataJson);
            //int category = Convert.ToInt32(jo["category"].ToString());
            //int pageIndex = Convert.ToInt32(jo["pageIndex"].ToString());

            FuncResult funcResult = new FuncResult();
            funcResult.Result = dataJson;
            funcResult.UserId = userId;
            funcResult.IsOver = false;
            funcResult.IsLead = 1;
            db.FuncResult.Add(funcResult);
            db.Entry(funcResult).State = System.Data.Entity.EntityState.Added;
            int result = db.SaveChanges();
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }
    }
}