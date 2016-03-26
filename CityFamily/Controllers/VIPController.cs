using CityFamily.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CityFamily.Controllers
{
    public class VIPController : Controller
    {
        private CityFamilyDbContext db = new CityFamilyDbContext();

        int pageSize = 8;
        public ActionResult FuncList(int lead = 1)
        {
            if (Session["userName"] != null)
            {

                int userId = (int)Session["userId"];
                List<FuncResult> funcResultList = new List<FuncResult>();
                List<JsonData> jsonDataList = new List<JsonData>();
                if (lead == 1)
                {
                    funcResultList = db.FuncResult.Where(item => item.UserId == userId).OrderByDescending(item => item.Id).ToList();
                }
                else
                {
                    funcResultList = db.FuncResult.Where(item => item.UserId == userId && item.IsLead == lead).OrderByDescending(item => item.Id).ToList();
                }
                //if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userPhone))
                //{
                //    funcResultList = db.FuncResult.Where(item => item.UserId == userId && item.IsLead == lead && item.GuestName.Contains(userName) && item.GuestPhone.Contains(userPhone)).OrderByDescending(item => item.Id).ToList();
                //    int count = funcResultList.Count();
                //    InitPage(userName, userPhone, pageIndex, count, lead);
                //    funcResultList = funcResultList.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                //}
                //else if (string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userPhone))
                //{
                //    funcResultList = db.FuncResult.Where(item => item.UserId == userId && item.IsLead == lead && item.GuestPhone.Contains(userPhone)).OrderByDescending(item => item.Id).ToList();
                //    int count = funcResultList.Count();
                //    InitPage(userName, userPhone, pageIndex, count, lead);
                //    funcResultList = funcResultList.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                //}
                //else if (!string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(userPhone))
                //{
                //    funcResultList = db.FuncResult.Where(item => item.UserId == userId && item.IsLead == lead && item.GuestName.Contains(userName)).OrderByDescending(item => item.Id).ToList();
                //    int count = funcResultList.Count();
                //    InitPage(userName, userPhone, pageIndex, count, lead);
                //    funcResultList = funcResultList.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                //}
                //else
                //{
                //    funcResultList = db.FuncResult.Where(item => item.UserId == userId && item.IsLead == lead).OrderByDescending(item => item.Id).ToList();
                //    int count = funcResultList.Count();
                //    InitPage(userName, userPhone, pageIndex, count, lead);
                //    funcResultList = funcResultList.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                //}
                foreach (FuncResult funcResult in funcResultList)
                {
                    JsonData data = ScriptDeserialize(funcResult.Result);
                    jsonDataList.Add(data);
                }
                VIPListView view = new VIPListView();
                view.funcResult = funcResultList;
                view.jsonData = jsonDataList;
                ViewBag.IsLead = lead;
                return View(view);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        ///////////////////////////////////////添加搜索userName、userPhone参数，pageIndex分页参数///////////////////////////////////////////////////////////////////////
        //public ActionResult FuncList(string userName, string userPhone, int lead = 1,int pageIndex = 1)
        //{          
        //    if (Session["userName"] != null)
        //    {

        //        int userId = (int)Session["userId"];
        //        List<FuncResult> funcResultList = new List<FuncResult>();
        //        List<JsonData> jsonDataList = new List<JsonData>();
        //        //if (lead == 1)
        //        //{
        //        //    funcResultList = db.FuncResult.Where(item => item.UserId == userId).OrderByDescending(item => item.Id).ToList();
        //        //}
        //        //else
        //        //{
        //            //funcResultList = db.FuncResult.Where(item => item.UserId == userId && item.IsLead == lead).OrderByDescending(item => item.Id).ToList();
        //        //}
        //        if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userPhone))
        //        {
        //            funcResultList = db.FuncResult.Where(item => item.UserId == userId && item.IsLead == lead && item.GuestName.Contains(userName) && item.GuestPhone.Contains(userPhone)).OrderByDescending(item => item.Id).ToList();
        //            int count = funcResultList.Count();
        //            InitPage(userName,userPhone, pageIndex, count, lead);
        //            funcResultList = funcResultList.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
        //        }
        //        else if(string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userPhone))
        //        {
        //            funcResultList = db.FuncResult.Where(item => item.UserId == userId && item.IsLead == lead && item.GuestPhone.Contains(userPhone)).OrderByDescending(item => item.Id).ToList();
        //            int count = funcResultList.Count();
        //            InitPage(userName, userPhone, pageIndex, count, lead);
        //            funcResultList = funcResultList.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
        //        }
        //        else if(!string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(userPhone))
        //        {
        //            funcResultList = db.FuncResult.Where(item => item.UserId == userId && item.IsLead == lead && item.GuestName.Contains(userName)).OrderByDescending(item => item.Id).ToList();
        //            int count = funcResultList.Count();
        //            InitPage(userName, userPhone, pageIndex, count, lead);
        //            funcResultList = funcResultList.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
        //        }
        //        else
        //        {
        //            funcResultList = db.FuncResult.Where(item => item.UserId == userId && item.IsLead == lead).OrderByDescending(item => item.Id).ToList();
        //            int count = funcResultList.Count();
        //            InitPage(userName, userPhone, pageIndex, count, lead);
        //            funcResultList = funcResultList.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
        //        }
        //        foreach (FuncResult funcResult in funcResultList)
        //        {
        //            JsonData data = ScriptDeserialize(funcResult.Result);
        //            jsonDataList.Add(data);
        //        }
        //        VIPListView view = new VIPListView();
        //        view.funcResult = funcResultList;
        //        view.jsonData = jsonDataList;
        //        ViewBag.IsLead = lead;
        //        return View(view);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Login", "Home");
        //    }
        //}
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //private void InitPage(string userName, string userPhone, int pageIndex, int count, int lead)
        //{
        //    int pageCount = (count % pageSize == 0) ? count / pageSize : count / pageSize + 1;
        //    if (pageCount == 0)
        //    {
        //        pageCount = 1;
        //    }
        //    string perPage = Url.Action("FuncList", new { userName = userName, userPhone = userPhone, lead = lead, pageIndex = (pageIndex < 2) ? 1 : pageIndex - 1 });
        //    string nextPage = Url.Action("FuncList", new { userName = userName, userPhone = userPhone, lead = lead, pageIndex = (pageIndex == pageCount) ? pageCount : (pageIndex + 1) });
        //    string lastPage = Url.Action("FuncList", new { userName = userName, userPhone = userPhone, lead = lead, pageIndex = pageCount });
        //    string firstPage = Url.Action("FuncList", new { userName = userName, userPhone = userPhone, lead = lead, pageIndex = 1 });
        //    string pageX = Url.Action("FuncList", new { userName = userName, userPhone = userPhone, lead = lead, pageIndex });
        //    ViewBag.perPage = perPage;
        //    ViewBag.nextPage = nextPage;
        //    ViewBag.pageCount = pageCount;
        //    ViewBag.pageX = pageX.Substring(pageX.Length - 1, 1);
        //    ViewBag.lastPage = lastPage;
        //    ViewBag.firstPage = firstPage;
        //}
        public ActionResult DIYList()
        {
            if (Session["userName"] != null)
            {
                int userId = (int)Session["userId"];
                var diyList = db.DIYResult.Where(item => item.UserId == userId).OrderByDescending(item => item.Id);
                return View(diyList);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        public ActionResult FuncDetails(int id)
        {
            if (Session["userName"] != null)
            {
                var funResult = db.FuncResult.Find(id);
                JsonData data = ScriptDeserialize(funResult.Result);
                ViewBag.IsLead = funResult.IsLead;
                ViewBag.Id = id;
                return View(data);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }


        public ActionResult EditLead(int id, int lead)
        {
            if (Session["userName"] != null)
            {
                var funResult = db.FuncResult.Find(id);
                funResult.IsLead = lead;
                db.Entry(funResult).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("FuncDetails", new { id = id });
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }


        public ActionResult DIYDetail(int id)
        {
            if (Session["userName"] != null)
            {
                var diyResult = db.DIYResult.Find(id);
                StyleThird styleThird = db.StyleThird.Find(diyResult.StyleDetailId);
                ViewBag.styleCode = styleThird.StyleThirdCode;
                ViewBag.styleResource = styleThird.StyleResource;
                return View(diyResult);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult SaveDIY(int id, string diyJson, string userName, string comment)
        {
            var diy = db.DIYResult.Find(id);
            diy.DIYJson = diyJson;
            diy.CreateTime = DateTime.Now.ToString("yyyy-MM-dd");
            diy.GuestName = userName;
            diy.Remark = comment;
            db.Entry(diy).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("StyleDetail", "Styles", new { id = diy.StyleDetailId });
        }

        [HttpPost]
        public ActionResult DeleteFunc()
        {
            if (Session["userName"] != null)
            {
                var stream = HttpContext.Request.InputStream;
                string dataJson = new StreamReader(stream).ReadToEnd(); //json 字符串在此
                int id = 0;
                int.TryParse(dataJson, out id);
                FuncResult result = db.FuncResult.Find(id);
                if (result != null)
                {
                    db.FuncResult.Remove(result);
                    db.SaveChanges();
                    return Content("Success");
                }
                else
                {
                    return Content("Error");
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        [HttpPost]
        public ActionResult DeleteDIY()
        {
            if (Session["userName"] != null)
            {
                var userName = System.Web.HttpContext.Current.Request["username"];
                var stream = HttpContext.Request.InputStream;
                string dataJson = new StreamReader(stream).ReadToEnd(); //json 字符串在此
                int id = 0;
                int.TryParse(dataJson, out id);
                DIYResult result = db.DIYResult.Find(id);
                if (result != null)
                {
                    db.DIYResult.Remove(result);
                    db.SaveChanges();
                    return Content("Success");
                }
                else
                {
                    return Content("Error");
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        public static JsonData ScriptDeserialize(string strJson)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Deserialize<JsonData>(strJson);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>
        /// {
        ///     "data":{
        ///         "FuncResultStateOne":[],
        ///         "FuncResultStateTwo":[
        ///             {
        ///                 "Id":84,
        ///                 "Name":"晕",
        ///                 "Phone":"123",
        ///                 "XiaoQu":"啊热舞vc",
        ///                 "Usage":"投资出租-",
        ///                 "Work":"医生-老板-"
        ///             }
        ///         ],
        ///         "FuncResultStateThree":[
        ///             {
        ///                 "Id":86,
        ///                 "Name":"dfsad官方订购",
        ///                 "Phone":"适当提高到法国",
        ///                 "XiaoQu":"方大同共同富裕",
        ///                 "Usage":"自己居住-",
        ///                 "Work":null
        ///             },
        ///             {
        ///                 "Id":93,
        ///                 "Name":"晕",
        ///                 "Phone":"123",
        ///                 "XiaoQu":"啊热舞vc",
        ///                 "Usage":"投资出租-",
        ///                 "Work":"医生-老板-"
        ///             }
        ///         ]
        ///     }
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult GetResultList(int userId)
        {
            List<FuncResult> resultListOne = db.FuncResult.Where(item => item.UserId == userId && item.IsLead == 1).ToList();
            List<FuncResult> resultListTwo = db.FuncResult.Where(item => item.UserId == userId && item.IsLead == 2).ToList();
            List<FuncResult> resultListThree = db.FuncResult.Where(item => item.UserId == userId && item.IsLead == 3).ToList();

            FuncResultListData resultDataList = new FuncResultListData();

            List<FuncResultStateOne> resultDataStateOne = new List<FuncResultStateOne>();
            List<FuncResultStateTwo> resultDataStateTwo = new List<FuncResultStateTwo>();
            List<FuncResultStateThree> resultDataStateThree = new List<FuncResultStateThree>();

            foreach (FuncResult result in resultListOne)
            {
                FuncResultStateOne resultData = new FuncResultStateOne();
                JObject jo = (JObject)JsonConvert.DeserializeObject(result.Result);
                resultData.Id = result.Id;
                resultData.Name = jo["Name"].ToString();
                resultData.Phone = jo["Phone"].ToString();
                resultData.XiaoQu = jo["Xiaoqu"].ToString();
                for(int i=0;i< jo["Usage"].ToArray().Length; i++)
                {
                    resultData.Usage += jo["Usage"][i]["Name"] + " ";
                }
                for (int i = 0; i < jo["Work"].ToArray().Length; i++)
                {
                    if (jo["Work"][i]["Name"] != null)
                    {

                        resultData.Work += jo["Work"][i]["Name"] + " ";
                    }
                }
                resultDataStateOne.Add(resultData);
            }

            foreach (FuncResult result in resultListTwo)
            {
                FuncResultStateTwo resultData = new FuncResultStateTwo();
                JObject jo = (JObject)JsonConvert.DeserializeObject(result.Result);
                resultData.Id = result.Id;
                resultData.Name = jo["Name"].ToString();
                resultData.Phone = jo["Phone"].ToString();
                resultData.XiaoQu = jo["Xiaoqu"].ToString();
                for (int i = 0; i < jo["Usage"].ToArray().Length; i++)
                {
                    resultData.Usage += jo["Usage"][i]["Name"] + " ";
                }
                for (int i = 0; i < jo["Work"].ToArray().Length; i++)
                {
                    if (jo["Work"][i]["Name"] != null)
                    {

                        resultData.Work += jo["Work"][i]["Name"] + " ";
                    }
                }
                resultDataStateTwo.Add(resultData);
            }
            foreach (FuncResult result in resultListThree)
            {
                FuncResultStateThree resultData = new FuncResultStateThree();
                JObject jo = (JObject)JsonConvert.DeserializeObject(result.Result);
                resultData.Id = result.Id;
                resultData.Name = jo["Name"].ToString();
                resultData.Phone = jo["Phone"].ToString();
                resultData.XiaoQu = jo["Xiaoqu"].ToString();
                for (int i = 0; i < jo["Usage"].ToArray().Length; i++)
                {
                    resultData.Usage += jo["Usage"][i]["Name"] + " ";
                }
                for (int i = 0; i < jo["Work"].ToArray().Length; i++)
                {
                    if (jo["Work"][i]["Name"] != null)
                    {

                        resultData.Work += jo["Work"][i]["Name"] + " ";
                    }
                }
                resultDataStateThree.Add(resultData);
            }

            resultDataList.FuncResultStateOne = resultDataStateOne;
            resultDataList.FuncResultStateTwo = resultDataStateTwo;
            resultDataList.FuncResultStateThree = resultDataStateThree;

            return Json(new { data = resultDataList });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultId"></param>
        /// <returns>
        /// {
        ///     "data":{
        ///         "Id":93,
        ///         "UserId":19,
        ///         "Result":"{\"Name\":\"晕\",\"Age\":\"\",\"Usage\":[{\"Name\":\"投资出租\"}],\"Work\":[{\"Name\":\"医生\"},{\"Name\":\"老板\"},{\"Other\":\"啊放假l\u0027kei\"}],\"Address\":\"色t\u0027g\u0027r区/县 - se\u0027te街道 - e\u0027s\u0027t\u0027g小区 - 扼杀t\u0027r单元 - 饿死t\u0027s室\",\"Phone\":\"123\",\"Xiaoqu\":\"啊热舞vc\",\"Anniversary\":[],\"GoodsHad\":[{\"Name\":\"涩谷t\u0027s\",\"Size\":\"石头t\",\"Position\":\"睡过头r\u0027se\"},{\"Name\":\"se\u0027g\u0027r\u0027s\",\"Size\":\"饿填t\",\"Position\":\"身体r\u0027se\"}],\"Family\":[{\"Name\":\"吧\",\"Age\":\"13\",\"Work\":\"e\u0027t\u0027t\",\"Hobby\":\"fret\u0027v\",\"Birthday\":\"给发个\"},{\"Name\":\"图文\",\"Age\":\"43台湾\",\"Work\":\"痛w\u0027t\",\"Hobby\":\"无一条3w\u0027t\",\"Birthday\":\"3外套w\"}],\"AgreedTime\":\"2016-3-2\",\"Equipment\":[{\"Name\":\"新风系统\"},{\"Name\":\"软水系统\"}],\"Intrest\":[{\"Name\":\"音乐\"},{\"Name\":\"旅行\"},{\"Other\":\"恶vtst\"}],\"Material\":[{\"Name\":\"人造大理石\"},{\"Name\":\"木质饰面板\"},{\"Name\":\"壁纸\"}],\"MaterialNo\":[{\"Name\":\"实木\"}],\"MaterialOther\":\"vegas人员不r\u0027b\u0027y\",\"FurnitureViewUrl\":\"\",\"diyResultUrl\":\"\",\"Space\":[{\"Name\":\"门厅\",\"Other\":\"输入vesb\"},{\"Name\":\"客厅\",\"Other\":\"色vrtstv\"},{\"Name\":\"餐厅\",\"Other\":\"vrest\u0027vest\u0027v\"},{\"Name\":\"主卧室\",\"Other\":\"涩痛vs\"},{\"Name\":\"老人房\",\"Other\":\"色图vs\u0027s\"},{\"Name\":\"儿童房\",\"Other\":\"瑟vtgr\"},{\"Name\":\"厨房\",\"Other\":\"se\u0027r\u0027t\u0027vgestg\"},{\"Name\":\"卫生间\",\"Other\":\"额vs通过4\"},{\"Name\":\"储物间(衣帽间)\",\"Other\":\"额vtt\"},{\"Name\":\"多功能空间\",\"Other\":\"而她为别vtw\"}]}",
        ///         "IsOver":false,
        ///         "IsLead":1
        ///     }
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult GetResultDetail(int resultId)
        {
            FuncResult funcResult = db.FuncResult.Find(resultId);
            return Json(new { data = funcResult }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult VipToLiangFang(int resultId)
        {
            FuncResult funcResult = db.FuncResult.Find(resultId);
            funcResult.IsLead = 2;
            db.Entry(funcResult).State = EntityState.Modified;
            int result = db.SaveChanges();
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult VipToQianYue(int resultId)
        {
            FuncResult funcResult = db.FuncResult.Find(resultId);
            funcResult.IsLead = 3;
            db.Entry(funcResult).State = EntityState.Modified;
            int result = db.SaveChanges();
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }
    }
}