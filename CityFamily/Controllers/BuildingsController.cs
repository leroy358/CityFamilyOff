using CityFamily.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityFamily.Controllers
{
    public class BuildingsController : Controller
    {
        private CityFamilyDbContext db = new CityFamilyDbContext();

        GetWebSession websession = new GetWebSession();
        int pageSize = 8;

        //如用表单提交方式，则直接返回数据至视图
        public ActionResult List(string province, string city, string town, string searchStr,int pageIndex = 1)
        {
            /////////////////////////////////////////////////////////////////////////////////添加pageIndex参数/////////////////////////////////////////////////////////////////////////////////

            if (Session["userName"] != null)
            {
                int adminId = websession.AdminId;
                int companyId = websession.CompanyId;
                //    省是否为空
                bool isProvince = (province == "省份" || string.IsNullOrEmpty(province));
                //    市是否为空
                bool isCity = (city == "地级市" || string.IsNullOrEmpty(city));
                //    区是否为空
                bool isDistrict = (town == "区/县" || string.IsNullOrEmpty(town));
                //    搜索条件是否为空
                bool isSearch = string.IsNullOrEmpty(searchStr);

                //    省、市、区筛选条件、搜索内容都不为空
                if (!isProvince && !isCity && !isDistrict && !isSearch)
                {
                    var buildings = db.Building.Where(item =>(item.CreateUserId == 1 || item.CompanyId == companyId) && item.Province == province && item.City == city && item.District == town && item.BuildingName.Contains(searchStr));
                    int count = buildings.Count();
                    InitPage(province, city, town, searchStr, pageIndex, count);
                    buildings = buildings.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                    return View(buildings);
                }

                //    省、市、区筛选条件不为空、搜索内容为空
                else if (!isProvince && !isCity && !isDistrict && isSearch)
                {
                    var buildings = db.Building.Where(item => (item.CreateUserId == 1 || item.CompanyId == companyId) && item.Province == province && item.City == city && item.District == town);
                    int count = buildings.Count();
                    InitPage(province, city, town, searchStr, pageIndex, count);
                    buildings = buildings.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                    return View(buildings);
                }

                //    省、市筛选条件不为空，区为空，搜索内容不为空
                else if (!isProvince && !isCity && isDistrict && !isSearch)
                {
                    var buildings = db.Building.Where(item => (item.CreateUserId == 1 || item.CompanyId == companyId) && item.Province == province && item.City == city && item.BuildingName.Contains(searchStr));
                    int count = buildings.Count();
                    InitPage(province, city, town, searchStr, pageIndex, count);
                    buildings = buildings.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                    return View(buildings);
                }

                //      省、市筛选条件不为空，区为空，搜索内容为空
                else if (!isProvince && !isCity && isDistrict && isSearch)
                {
                    var buildings = db.Building.Where(item => (item.CreateUserId == 1 || item.CompanyId == companyId) && item.Province == province && item.City == city);
                    int count = buildings.Count();
                    InitPage(province, city, town, searchStr, pageIndex, count);
                    buildings = buildings.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                    return View(buildings);
                }

                //      省筛选不为空，市、区筛选为空，搜索内容不为空
                else if (!isProvince && isCity && isDistrict && !isSearch)
                {
                    var buildings = db.Building.Where(item => (item.CreateUserId == 1 || item.CompanyId == companyId) && item.Province == province && item.BuildingName.Contains(searchStr));
                    int count = buildings.Count();
                    InitPage(province, city, town, searchStr, pageIndex, count);
                    buildings = buildings.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                    return View(buildings);
                }

                //      省筛选不为空，市、区筛选为空，搜索内容为空
                else if (!isProvince && isCity && isDistrict && isSearch)
                {
                    var buildings = db.Building.Where(item => (item.CreateUserId == 1 || item.CompanyId == companyId) && item.Province == province);
                    int count = buildings.Count();
                    InitPage(province, city, town, searchStr, pageIndex, count);
                    buildings = buildings.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                    return View(buildings);
                }

                //    省、市、区筛选为空，搜索内容不为空  
                else if (isProvince && isCity && isDistrict && !isSearch)
                {
                    var buildings = db.Building.Where(item => (item.CreateUserId == 1 || item.CompanyId == companyId) && item.BuildingName.Contains(searchStr));
                    int count = buildings.Count();
                    InitPage(province, city, town, searchStr, pageIndex, count);
                    buildings = buildings.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                    return View(buildings);
                }

                //      省、市、区筛选为空，搜索内容为空 
                else
                {
                    var buildings = db.Building.Where(item => (item.CreateUserId == 1 || item.CompanyId == companyId));
                    int count = buildings.Count();
                    InitPage(province, city, town, searchStr, pageIndex, count);
                    buildings = buildings.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                    return View(buildings);
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }


        //若用AJAX POST方法请求，则返回如下JSON数据：
        //{
        //  "data":
        //      [
        //          {
        //              "Id":6,
        //              "Province":"江苏省",
        //              "City":"无锡市",
        //              "District":"崇安区",
        //              "BuildingName":"红星国际广场",
        //              "BuildingIndex":"/Images/data/201505/201505201757555750.jpg",
        //              "BuildingPics":"/Images/data/201505/201505201758015420.jpg /Images/data/201505/201505201758015431.jpg ",
        //              "BuildingIntro":"商住两用",
        //              "BuildingAD":"锡沪路1800号",
        //              "BuildingCate":"多层  小高层  高层",
        //              "BuildingFeature":"高层",
        //              "BuildingDecorate":"毛胚  精装修",
        //              "CreateTime":"2015-05-25"
        //          },
        //          {
        //              "Id":1,
        //              "Province":"江苏省",
        //              "City":"无锡市",
        //              "District":"北塘区",
        //              "BuildingName":"维多利亚橄榄城",
        //              "BuildingIndex":"/Images/data/201505/201505211029405320.jpg",
        //              "BuildingPics":"/Images/data/201505/201505201422142560.jpg /Images/data/201505/201505201422142571.jpg /Images/data/201505/201505201422142732.jpg /Images/data/201505/201505201422142743.jpg ",
        //              "BuildingIntro":"商住两用",
        //              "BuildingAD":"南长区人民西路1800号（人民路与中山路交汇处往南200米）",
        //              "BuildingCate":"多层  小高层  高层",
        //              "BuildingFeature":"特色别墅  创意地产",
        //              "BuildingDecorate":"毛胚  精装修",
        //              "CreateTime":"2015-05-21"
        //          }
        //      ]
        //}

        //[HttpPost]
        //public ActionResult List(string province, string city, string district)
        //{
        //    bool isProvince = string.IsNullOrEmpty(province);
        //    bool isCity = string.IsNullOrEmpty(city);
        //    bool isDistrict = string.IsNullOrEmpty(district);
        //      //省、市、区筛选条件都不为空
        //    if (!isProvince && !isCity && !isDistrict)
        //    {
        //        var buildings = db.Building.Where(item => item.Province == province && item.City == city && item.District == district).OrderByDescending(item => item.Id);
        //        return Json(new { data = buildings }, JsonRequestBehavior.AllowGet);
        //    }
        //    //    省、市筛选条件都不为空，区为空
        //    else if (!isProvince && !isCity && isDistrict)
        //    {
        //        var buildings = db.Building.Where(item => item.Province == province && item.City == city).OrderByDescending(item => item.Id);
        //        return Json(new { data = buildings }, JsonRequestBehavior.AllowGet); 
        //    }
        //      //    省筛选条件都不为空，市、区为空
        //    else if (!isProvince && isCity)
        //    {
        //        var buildings = db.Building.Where(item => item.Province == province).OrderByDescending(item => item.Id);
        //        return Json(new { data = buildings }, JsonRequestBehavior.AllowGet); 
        //    }
        //    //    省、市、区筛选条件都为空
        //    else
        //    {
        //        var buildings = db.Building.OrderByDescending(item => item.Id);
        //        return Json(new { data = buildings }, JsonRequestBehavior.AllowGet); 
        //    }
        //}
        public ActionResult BuildingDetails(int id)
        {
            if (Session["userName"] != null)
            {
                BuildingLayoutView view = new BuildingLayoutView();
                Building building = db.Building.Find(id);
                var layouts = db.Layout.Where(item => item.BuildingId == id);
                view.building = building;
                view.layouts = layouts.ToList();
                return View(view);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////

        private void InitPage(string province, string city, string town, string searchStr, int pageIndex, int count)
        {
            int pageCount = (count % pageSize == 0) ? count / pageSize : count / pageSize + 1;
            if (pageCount == 0)
            {
                pageCount = 1;
            }
            string perPage = Url.Action("List", new { province = province, city = city, town = town, searchStr = searchStr, pageIndex = (pageIndex < 2) ? 1 : pageIndex - 1 });
            string nextPage = Url.Action("List", new { province = province, city = city, town = town, searchStr = searchStr, pageIndex = (pageIndex == pageCount) ? pageCount : (pageIndex + 1) });
            string lastPage = Url.Action("List", new { province = province, city = city, town = town, searchStr = searchStr, pageIndex = pageCount });
            string firstPage = Url.Action("List", new { province = province, city = city, town = town, searchStr = searchStr, pageIndex = 1 });
            string pageX = Url.Action("List", new { province = province, city = city, town = town, searchStr = searchStr,  pageIndex = pageIndex });
            ViewBag.perPage = perPage;
            ViewBag.nextPage = nextPage;
            ViewBag.pageCount = pageCount;
            ViewBag.pageX = pageX.Substring(pageX.Length - 1, 1);
            ViewBag.lastPage = lastPage;
            ViewBag.firstPage = firstPage;
        }
    }
}