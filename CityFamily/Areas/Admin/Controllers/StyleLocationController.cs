using CityFamily.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityFamily.Areas.Admin.Controllers
{
    public class StyleLocationController : Controller
    {
        private CityFamilyDbContext db = new CityFamilyDbContext();
        int pageSize = 20;
        public ActionResult DIYList(string searchStr, int pageIndex = 1)
        {
            if (Session["admin"] != null)
            {
                if (string.IsNullOrEmpty(searchStr))
                {
                    var diyResult = db.DIYResult.OrderByDescending(item => item.Id);
                    int count = diyResult.Count();
                    InitPage(pageIndex, count, searchStr);
                    return View(diyResult);
                }
                else
                {
                    ViewBag.searchStr = searchStr;
                    var diyResult = db.DIYResult.Where(item => item.GuestName.Contains(searchStr) || item.UserName.Contains(searchStr)).OrderByDescending(item => item.Id);
                    int count = diyResult.Count();
                    InitPage(pageIndex, count, searchStr);
                    return View(diyResult);
                }
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }

        public ActionResult DIYDetail(int id)
        {
            if (Session["admin"] != null)
            {
                DIYResult result = db.DIYResult.Find(id);
                StyleThird styleThird = db.StyleThird.Find(result.StyleDetailId);
                ViewBag.styleCode = styleThird.StyleThirdCode;
                ViewBag.styleResource = styleThird.StyleResource;
                return View(result);
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
            string perPage = Url.Action("DIYList", new { searchStr, pageIndex = (pageIndex < 2) ? 1 : pageIndex - 1 });
            string nextPage = Url.Action("DIYList", new { searchStr, pageIndex = (pageIndex == pageCount) ? pageCount : (pageIndex + 1) });
            string lastPage = Url.Action("DIYList", new { searchStr, pageIndex = pageCount });
            string firstPage = Url.Action("DIYList", new { searchStr, pageIndex = 1 });
            string pageX = Url.Action("DIYList", new { searchStr, pageIndex });
            ViewBag.perPage = perPage;
            ViewBag.nextPage = nextPage;
            ViewBag.pageCount = pageCount;
            ViewBag.pageX = pageX.Substring(pageX.Length - 1, 1);
            ViewBag.lastPage = lastPage;
            ViewBag.firstPage = firstPage;
        }
	}
}