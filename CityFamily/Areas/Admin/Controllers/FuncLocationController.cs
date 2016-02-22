using CityFamily.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CityFamily.Areas.Admin.Controllers
{
    public class FuncLocationController : Controller
    {
        private CityFamilyDbContext db = new CityFamilyDbContext();
        int pageSize = 10;
        public ActionResult List(int lead, string searchStr, int pageIndex = 1)
        {
            if (Session["admin"] != null)
            {
                int adminid = int.Parse(Session["adminId"].ToString());

                FuncListView funcView = new FuncListView();
                List<FuncResult> resultsList = new List<FuncResult>();
                List<JsonData> dataList = new List<JsonData>();
                List<Admins> userList = new List<Admins>();
                List<int> td = null;
                IOrderedQueryable<FuncResult> results = null;
                int count = 0;

                if (adminid == 1)
                {
                    var frdata = db.FuncResult.Where(o => o.IsLead == lead).ToList();
                    if (frdata.Count > 0)
                    {
                        td = db.FuncResult.Where(o => o.IsLead == lead).Select(o => o.UserId).ToList();
                    }
                }
                else
                {
                    List<int> up = db.T_UserDepartment.Where(o => o.UserID == adminid).Select(o => o.DepartmentID).ToList();
                    td = db.T_UserDepartment.Where(o => up.Contains(o.DepartmentID)).Select(o => o.UserID).ToList();
                }

                if (td != null)
                {
                    results = db.FuncResult.Where(o => td.Contains(o.UserId) && o.IsLead == lead).OrderByDescending(item => item.Id);

                    if (!string.IsNullOrEmpty(searchStr))
                    {
                        foreach (FuncResult result in results)
                        {
                            var jsonobj = ScriptDeserialize(result.Result);
                            if (!string.IsNullOrEmpty(jsonobj.Name) && !string.IsNullOrEmpty(jsonobj.XiaoQu))
                            {
                                if (jsonobj.Name.Contains(searchStr) || jsonobj.XiaoQu.Contains(searchStr))
                                {
                                    resultsList.Add(result);
                                }
                                else if (db.Admins.Find(result.UserId) != null)
                                {
                                    if (db.Admins.Find(result.UserId).AdminName.Contains(searchStr))
                                    {
                                        resultsList.Add(result);
                                    }
                                }
                            }
                        }
                        count = resultsList.Count();
                        resultsList = resultsList.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();

                    }
                    else
                    {
                        resultsList = results.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                        count = db.FuncResult.Count();
                    }
                    funcView.FuncResult = resultsList.OrderByDescending(item => item.Id).ToList();
                    foreach (FuncResult result in funcView.FuncResult)
                    {
                        dataList.Add(ScriptDeserialize(result.Result));
                        userList.Add(db.Admins.Find(result.UserId));
                    }
                    funcView.DataResult = dataList;
                    funcView.User = userList;
                }
                InitPage(pageIndex, count, searchStr);
                ViewBag.searchStr = searchStr;
                ViewBag.IsLead = lead;
                return View(funcView);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }

        public ActionResult Details(int id)
        {
            if (Session["admin"] != null)
            {
                FuncResult result = db.FuncResult.Find(id);
                JsonData jsonResult = ScriptDeserialize(result.Result);
                ViewBag.GuestName = jsonResult.Name;
                ViewBag.IsLead = result.IsLead;
                return View(jsonResult);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        private void InitPage(int pageIndex, int count, string searchStr)
        {
            int pageCount = (count % pageSize == 0) ? count / pageSize : count / pageSize + 1;
            if (pageCount == 0)
            {
                pageCount = 1;
            }
            string perPage = Url.Action("List", new { searchStr, pageIndex = (pageIndex < 2) ? 1 : pageIndex - 1 });
            string nextPage = Url.Action("List", new { searchStr, pageIndex = (pageIndex == pageCount) ? pageCount : (pageIndex + 1) });
            string lastPage = Url.Action("List", new { searchStr, pageIndex = pageCount });
            string firstPage = Url.Action("List", new { searchStr, pageIndex = 1 });
            string pageX = Url.Action("List", new { searchStr, pageIndex });
            ViewBag.perPage = perPage;
            ViewBag.nextPage = nextPage;
            ViewBag.pageCount = pageCount;
            ViewBag.pageX = pageX.Substring(pageX.Length - 1, 1);
            ViewBag.lastPage = lastPage;
            ViewBag.firstPage = firstPage;
        }

        public static JsonData ScriptDeserialize(string strJson)
        {

            JavaScriptSerializer js = new JavaScriptSerializer();

            return js.Deserialize<JsonData>(strJson);
        }
    }
}