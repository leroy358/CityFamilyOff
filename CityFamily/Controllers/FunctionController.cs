using CityFamily.Models;
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
    }
}