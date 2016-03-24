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
        ///     "data":[
        ///         {
        ///             "Id":84,
        ///             "Name":"dfsad官方订购",
        ///             "Phone":"适当提高到法国",
        ///             "XiaoQu":"方大同共同富裕",
        ///             "Usage":"自己居住-",
        ///             "Work":null
        ///         },
        ///         {
        ///             "Id":86,
        ///             "Name":"dfsad官方订购",
        ///             "Phone":"适当提高到法国",
        ///             "XiaoQu":"方大同共同富裕",
        ///             "Usage":"自己居住-",
        ///             "Work":"医生-老板-"
        ///         },
        ///         {
        ///             "Id":93,
        ///             "Name":"dfsad官方订购","Phone":
        ///             "适当提高到法国","XiaoQu":"方大同共同富裕",
        ///             "Usage":"自己居住-",
        ///             "Work":null
        ///         }
        ///     ]
        /// }
        /// {"data":[{"Id":84,"Name":"晕","Phone":"123","XiaoQu":"啊热舞vc","Usage":"投资出租-","Work":"医生-老板-"},{"Id":86,"Name":"dfsad官方订购","Phone":"适当提高到法国","XiaoQu":"方大同共同富裕","Usage":"自己居住-","Work":null},{"Id":93,"Name":"晕","Phone":"123","XiaoQu":"啊热舞vc","Usage":"投资出租-","Work":"医生-老板--"}]}
        /// </returns>
        [HttpPost]
        public ActionResult GetResultList(int userId)
        {
            List<FuncResult> resultList = db.FuncResult.Where(item => item.UserId == userId).ToList();
            List<FuncResultListData> resultDataList = new List<FuncResultListData>();
            foreach(FuncResult result in resultList)
            {
                FuncResultListData resultData = new FuncResultListData();
                JObject jo = (JObject)JsonConvert.DeserializeObject(result.Result);
                resultData.Id = result.Id;
                resultData.Name = jo["Name"].ToString();
                resultData.Phone = jo["Phone"].ToString();
                resultData.XiaoQu = jo["Xiaoqu"].ToString();
                for(int i=0;i< jo["Usage"].ToArray().Length; i++)
                {
                    resultData.Usage += jo["Usage"][i]["Name"] + "-";
                }
                for (int i = 0; i < jo["Work"].ToArray().Length; i++)
                {
                    if (jo["Work"][i]["Name"] != null)
                    {

                        resultData.Work += jo["Work"][i]["Name"] + "-";
                    }
                }
                resultDataList.Add(resultData);
            }
            return Json(new { data = resultDataList });
        }
    }
}