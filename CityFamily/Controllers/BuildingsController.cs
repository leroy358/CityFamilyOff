using CityFamily.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        //public ActionResult List(string province, string city, string town, string searchStr, int pageIndex = 1)
        //{
        //    /////////////////////////////////////////////////////////////////////////////////添加pageIndex参数/////////////////////////////////////////////////////////////////////////////////

        //    if (Session["userName"] != null)
        //    {
        //        int adminId = websession.AdminId;
        //        int companyId = websession.CompanyId;
        //        //    省是否为空
        //        bool isProvince = (province == "省份" || string.IsNullOrEmpty(province));
        //        //    市是否为空
        //        bool isCity = (city == "地级市" || string.IsNullOrEmpty(city));
        //        //    区是否为空
        //        bool isDistrict = (town == "区/县" || string.IsNullOrEmpty(town));
        //        //    搜索条件是否为空
        //        bool isSearch = string.IsNullOrEmpty(searchStr);

        //        //    省、市、区筛选条件、搜索内容都不为空
        //        if (!isProvince && !isCity && !isDistrict && !isSearch)
        //        {
        //            var buildings = db.Building.Where(item => (item.CreateUserId == 1 || item.CompanyId == companyId) && item.Province == province && item.City == city && item.District == town && item.BuildingName.Contains(searchStr));
        //            int count = buildings.Count();
        //            InitPage(province, city, town, searchStr, pageIndex, count);
        //            buildings = buildings.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
        //            return View(buildings);
        //        }

        //        //    省、市、区筛选条件不为空、搜索内容为空
        //        else if (!isProvince && !isCity && !isDistrict && isSearch)
        //        {
        //            var buildings = db.Building.Where(item => (item.CreateUserId == 1 || item.CompanyId == companyId) && item.Province == province && item.City == city && item.District == town);
        //            int count = buildings.Count();
        //            InitPage(province, city, town, searchStr, pageIndex, count);
        //            buildings = buildings.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
        //            return View(buildings);
        //        }

        //        //    省、市筛选条件不为空，区为空，搜索内容不为空
        //        else if (!isProvince && !isCity && isDistrict && !isSearch)
        //        {
        //            var buildings = db.Building.Where(item => (item.CreateUserId == 1 || item.CompanyId == companyId) && item.Province == province && item.City == city && item.BuildingName.Contains(searchStr));
        //            int count = buildings.Count();
        //            InitPage(province, city, town, searchStr, pageIndex, count);
        //            buildings = buildings.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
        //            return View(buildings);
        //        }

        //        //      省、市筛选条件不为空，区为空，搜索内容为空
        //        else if (!isProvince && !isCity && isDistrict && isSearch)
        //        {
        //            var buildings = db.Building.Where(item => (item.CreateUserId == 1 || item.CompanyId == companyId) && item.Province == province && item.City == city);
        //            int count = buildings.Count();
        //            InitPage(province, city, town, searchStr, pageIndex, count);
        //            buildings = buildings.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
        //            return View(buildings);
        //        }

        //        //      省筛选不为空，市、区筛选为空，搜索内容不为空
        //        else if (!isProvince && isCity && isDistrict && !isSearch)
        //        {
        //            var buildings = db.Building.Where(item => (item.CreateUserId == 1 || item.CompanyId == companyId) && item.Province == province && item.BuildingName.Contains(searchStr));
        //            int count = buildings.Count();
        //            InitPage(province, city, town, searchStr, pageIndex, count);
        //            buildings = buildings.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
        //            return View(buildings);
        //        }

        //        //      省筛选不为空，市、区筛选为空，搜索内容为空
        //        else if (!isProvince && isCity && isDistrict && isSearch)
        //        {
        //            var buildings = db.Building.Where(item => (item.CreateUserId == 1 || item.CompanyId == companyId) && item.Province == province);
        //            int count = buildings.Count();
        //            InitPage(province, city, town, searchStr, pageIndex, count);
        //            buildings = buildings.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
        //            return View(buildings);
        //        }

        //        //    省、市、区筛选为空，搜索内容不为空  
        //        else if (isProvince && isCity && isDistrict && !isSearch)
        //        {
        //            var buildings = db.Building.Where(item => (item.CreateUserId == 1 || item.CompanyId == companyId) && item.BuildingName.Contains(searchStr));
        //            int count = buildings.Count();
        //            InitPage(province, city, town, searchStr, pageIndex, count);
        //            buildings = buildings.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
        //            return View(buildings);
        //        }

        //        //      省、市、区筛选为空，搜索内容为空 
        //        else
        //        {
        //            var buildings = db.Building.Where(item => (item.CreateUserId == 1 || item.CompanyId == companyId));
        //            int count = buildings.Count();
        //            InitPage(province, city, town, searchStr, pageIndex, count);
        //            buildings = buildings.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
        //            return View(buildings);
        //        }
        //    }
        //    else
        //    {
        //        return RedirectToAction("Login", "Home");
        //    }
        //}


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
        //public ActionResult BuildingDetails(int id)
        //{
        //    if (Session["userName"] != null)
        //    {
        //        BuildingLayoutView view = new BuildingLayoutView();
        //        Building building = db.Building.Find(id);
        //        var layouts = db.Layout.Where(item => item.BuildingId == id);
        //        view.building = building;
        //        view.layouts = layouts.ToList();
        //        return View(view);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Login", "Home");
        //    }
        //}
        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////

        //private void InitPage(string province, string city, string town, string searchStr, int pageIndex, int count)
        //{
        //    int pageCount = (count % pageSize == 0) ? count / pageSize : count / pageSize + 1;
        //    if (pageCount == 0)
        //    {
        //        pageCount = 1;
        //    }
        //    string perPage = Url.Action("List", new { province = province, city = city, town = town, searchStr = searchStr, pageIndex = (pageIndex < 2) ? 1 : pageIndex - 1 });
        //    string nextPage = Url.Action("List", new { province = province, city = city, town = town, searchStr = searchStr, pageIndex = (pageIndex == pageCount) ? pageCount : (pageIndex + 1) });
        //    string lastPage = Url.Action("List", new { province = province, city = city, town = town, searchStr = searchStr, pageIndex = pageCount });
        //    string firstPage = Url.Action("List", new { province = province, city = city, town = town, searchStr = searchStr, pageIndex = 1 });
        //    string pageX = Url.Action("List", new { province = province, city = city, town = town, searchStr = searchStr, pageIndex = pageIndex });
        //    ViewBag.perPage = perPage;
        //    ViewBag.nextPage = nextPage;
        //    ViewBag.pageCount = pageCount;
        //    ViewBag.pageX = pageX.Substring(pageX.Length - 1, 1);
        //    ViewBag.lastPage = lastPage;
        //    ViewBag.firstPage = firstPage;
        //}

        /// <summary>
        /// 获取楼盘列表页信息
        /// </summary>
        /// <param name="companyId">登录用户所属分公司ID</param>
        /// <returns>
        /// {
        ///     "data":[
        ///         {
        ///             "BuildingId":21,
        ///             "BuildingName":"轩苑紫郡",
        ///             "BuildingIndex":"/Images/data/201601/4b27b7ee-73eb-4d7b-8f54-a48ca0844b2e0.jpg"
        ///         },
        ///         {
        ///             "BuildingId":22,
        ///             "BuildingName":"温莎湖畔庄园",
        ///             "BuildingIndex":"/Images/data/201601/42d68f01-2842-479a-b7fa-772ffa24b9b70.jpg"
        ///         },
        ///         {
        ///             "BuildingId":23,
        ///             "BuildingName":"水晶七号",
        ///             "BuildingIndex":"/Images/data/201601/b5fef54b-4a00-45d1-8289-9052e79e23350.jpg"
        ///         }
        ///     ]
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult GetBuildingList(int companyId)
        {
            var buildings = db.Building.Where(item => (item.CreateUserId == 1 || item.CompanyId == companyId)).OrderByDescending(item => item.Id).Take(12);
            List<BuildingList> buildingList = new List<BuildingList>();
            foreach (Building building in buildings)
            {
                BuildingList buildingIndex = new BuildingList();
                buildingIndex.BuildingId = building.Id;
                buildingIndex.BuildingName = building.BuildingName;
                buildingIndex.BuildingIndex = ConfigurationManager.AppSettings["ResourceUrl"] + building.BuildingIndex;
                buildingList.Add(buildingIndex);
            }
            return Json(new { data = buildingList }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetBuildingListNext(int companyId, int buildingId)
        {
            var buildings = db.Building.OrderByDescending(item => item.Id).Where(item => item.Id < buildingId && (item.CreateUserId == 1 || item.CompanyId == companyId)).Take(12);
            List<BuildingList> buildingList = new List<BuildingList>();
            foreach (Building building in buildings)
            {
                BuildingList buildingIndex = new BuildingList();
                buildingIndex.BuildingId = building.Id;
                buildingIndex.BuildingName = building.BuildingName;
                buildingIndex.BuildingIndex = ConfigurationManager.AppSettings["ResourceUrl"] + building.BuildingIndex;
                buildingList.Add(buildingIndex);
            }
            return Json(new { data = buildingList }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取楼盘详情页信息
        /// </summary>
        /// <param name="buildingId"></param>
        /// <returns>
        /// {
        ///     "data":{
        ///         "BuildingId":1,
        ///         "BuildingName":"22222",
        ///         "BuildingPics":"/Images/data/201507/3385a56d-d561-4a0f-9033-99259ca172190.jpg /Images/data/201507/adb16dd1-c3cd-496f-8057-b482b37d4fca1.jpg /Images/data/201507/72cb6c36-a9a6-41a1-aa39-af8e49041e7e2.jpg /Images/data/201507/8e2c9b6b-004e-4270-b640-b3b5785434503.jpg /Images/data/201507/8713d2aa-8f9e-4957-872c-af61e69f346b4.jpg /Images/data/201507/d7542f53-fdaa-4930-ad8b-e9dc70051ab55.jpg ",
        ///         "BuildingIntro":"住宅、普通住宅",
        ///         "BuildingAD":"宝鸡市陈仓园二路",
        ///         "BuildingAroundPic":"/Images/data/201507/8b23c94d-00e5-4425-97d1-dbf11926dc810.jpg",
        ///         "BuildingDeco":"毛坯房",
        ///         "BuildingLayout":[
        ///             {
        ///                 "LayoutId":16,
        ///                 "LayoutName":"轩苑紫郡108㎡",
        ///                 "LayoutPic":"/Images/data/201507/0d74c585-f32b-4cff-9774-3a9bec8b4e400.jpg"
        ///             },
        ///             {
        ///                 "LayoutId":17,
        ///                 "LayoutName":"轩苑紫郡110㎡",
        ///                 "LayoutPic":"/Images/data/201507/7ebedc14-cb69-40f7-a285-e9bba51378ba0.jpg"
        ///             },
        ///             {
        ///                 "LayoutId":18,
        ///                 "LayoutName":"轩苑紫郡141㎡",
        ///                 "LayoutPic":"/Images/data/201507/4f2c4142-af1c-46f3-bb2a-742b865f00060.jpg"
        ///             }
        ///         ]
        ///     }
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult GetBuildingDetails(int buildingId)
        {
            Building building = db.Building.Find(buildingId);
            BuildingDetails buildingDetails = new BuildingDetails();
            buildingDetails.BuildingId = building.Id;
            buildingDetails.BuildingName = building.BuildingName;
            buildingDetails.BuildingPics = building.BuildingPics.Substring(0, building.BuildingPics.Length - 1).Split(' ');
            for(int i=0;i< buildingDetails.BuildingPics.Length; i++)
            {
                buildingDetails.BuildingPics[i]= ConfigurationManager.AppSettings["ResourceUrl"] + buildingDetails.BuildingPics[i];
            }
            buildingDetails.BuildingIntro = building.BuildingIntro;
            buildingDetails.BuildingAD = building.BuildingAD;
            buildingDetails.BuildingAroundPic = ConfigurationManager.AppSettings["ResourceUrl"] + building.BuildingCate;
            buildingDetails.BuildingDeco = building.BuildingDecorate;
            List<Layout> layoutList = db.Layout.Where(item => item.BuildingId == buildingId).ToList();
            List<BuildingLayout> buildingLayoutList = new List<BuildingLayout>();
            foreach (Layout layout in layoutList)
            {
                BuildingLayout buildingLayout = new BuildingLayout();
                buildingLayout.LayoutId = layout.Id;
                buildingLayout.LayoutName = layout.LayoutName;
                buildingLayout.LayoutPic = ConfigurationManager.AppSettings["ResourceUrl"] + layout.LayoutPic;
                buildingLayoutList.Add(buildingLayout);
            }
            buildingDetails.BuildingLayout = buildingLayoutList;
            return Json(new { data = buildingDetails }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据客户端最近更新时间判断服务器端是否有更新
        /// </summary>
        /// <param name="id">楼盘Id</param>
        /// <param name="updateTime">本地更新时间</param>
        /// <returns>
        /// 若有更新
        /// {
        ///     "IsUpdate":1
        /// }
        /// 若无更新
        /// {
        ///     "IsUpdate":0
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult IsBuildingUpdate(int id, string updateTime)
        {
            Building building = db.Building.Find(id);
            DateTime time = Convert.ToDateTime(updateTime);
            string buildingId = id.ToString();
            UpdateRecord record = db.UpdateRecord.Where(item => item.BuildingId == buildingId).FirstOrDefault();
            if (record.UpdateTime > time)
            {
                return Json(new { IsUpdate = 1 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { IsUpdate = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 根据客户端最近更新时间判断服务器端是否有更新，并批量获取楼盘下所有信息
        /// </summary>
        /// <param name="id">楼盘Id</param>
        /// <returns>
        /// {
        ///     "data":{
        ///         "BuildingId":4,
        ///         "BuildingName":"德阳-温莎湖畔庄园",
        ///         "BuildingIndex":"/Images/data/201601/8dcf05dc-0216-40b4-acd7-ec37c046ec520.jpg",
        ///         "BuildingPics":"/Images/data/201601/09c0f90f-8aa8-469c-b399-38883f5e9f200.jpg /Images/data/201601/860ef511-193b-4828-8ab1-3df5a62a65380.jpg ",
        ///         "BuildingIntro":"塔楼 高层 ",
        ///         "BuildingAD":"德阳市嘉陵江桥东岸（八一中学西侧100米处）",
        ///         "BuildingAroundPic":"/Images/data/201507/99cfdbd7-dfaf-43c9-aff2-bf59e3bbc1af0.jpg",
        ///         "BuildingDeco":"毛胚",
        ///         "BuildingUpdate":"2016-01-01 11:10:05",
        ///         "LayoutData":[
        ///             {
        ///                 "LayoutId":9,
        ///                 "LayoutName":"温莎湖畔庄园A1户型两室两厅一厨两卫91.24平米",
        ///                 "LayoutPic":"/Images/data/201507/d76ed0af-9456-449c-b55a-17fb3dcd72570.jpg",
        ///                 "LayoutAdvantages":"1、客厅开间4.05米，活动空间充足。\r\n2、南北双卧，互不干扰休息生活。\r\n3、整个房型分割较合理，布局紧凑，功能划分明确。\r\n4、主卧室朝南，冬暖夏凉，采光充足。厨房与主卧相隔较远，减少油烟味。",
        ///                 "LayoutDisadvantages":"1、起居厅的面积标准应大于卧室，\r\n2、厨房/卫生间的面积相应地也应大一些。\r\n3、门多，无稳定间，\r\n4、采光小或采光凹槽深，光线较暗，窗正对墙面，视野差、形状不好或尺度不合理。",
        ///                 "Decotate":[
        ///                     {
        ///                         "DecorateId":37,
        ///                         "DecorateIndex":"/Images/data/201507/2d7e78e6-96ab-4f15-8b41-749d72255f5b0.jpg",
        ///                         "DecoratePics":"/Images/data/201507/0a95b33c-7242-47c6-baca-8f8215ea08dd0.jpg ",
        ///                         "Decorate360":null
        ///                     },
        ///                     {
        ///                         "DecorateId":38,
        ///                         "DecorateIndex":"/Images/data/201507/4b0e576f-b680-46d0-a856-17c930eafd940.jpg",
        ///                         "DecoratePics":"/Images/data/201507/66878e71-8363-4b2d-b1fd-46775cbbf1080.jpg ",
        ///                         "Decorate360":null
        ///                     },
        ///                     {
        ///                         "DecorateId":45,
        ///                         "DecorateIndex":"/Images/data/201507/ae9abd2e-374d-4aa2-a5e0-91ba40d4f9cf0.jpg",
        ///                         "DecoratePics":null,
        ///                         "Decorate360":"http://119.188.126.108:8081/ZhuangXiu/DeYang/DeYang.html"
        ///                     }
        ///                 ],
        ///                 "SpotYuanZhuangData":[
        ///                     {
        ///                         "SpotId":21,
        ///                         "SpotIndex":"/Images/data/201507/d21af7a4-d5e3-432d-a973-2077345dfc440.jpg",
        ///                         "SpotPics":"/Images/data/201507/6f0915ac-6096-471d-b180-41eabb67308d0.jpg "
        ///                     },
        ///                     {
        ///                         "SpotId":47,
        ///                         "SpotIndex":"/Images/data/201507/ad5867f0-18e1-452f-9bd4-2513e2b0d1e60.png",
        ///                         "SpotPics":"/Images/data/201507/52a32a90-c4d3-46b8-af86-a25af1cc17440.png "
        ///                     }
        ///                 ],
        ///                 "SpotShiGongData":[
        ///                     {
        ///                         "SpotId":22,
        ///                         "SpotIndex":"/Images/data/201507/ad5867f0-18e1-452f-9bd4-2513e2b0d1e60.png",
        ///                         "SpotPics":"/Images/data/201507/52a32a90-c4d3-46b8-af86-a25af1cc17440.png "
        ///                     },
        ///                     {
        ///                         "SpotId":46,
        ///                         "SpotIndex":"/Images/data/201507/d21af7a4-d5e3-432d-a973-2077345dfc440.jpg",
        ///                         "SpotPics":"/Images/data/201507/6f0915ac-6096-471d-b180-41eabb67308d0.jpg "
        ///                     }
        ///                 ]
        ///             },
        ///             {
        ///                 "LayoutId":21,
        ///                 "LayoutName":"温莎湖畔庄园A2户型三室两厅一厨一卫96.53平米",
        ///                 "LayoutPic":"/Images/data/201507/26f6a7ea-314e-4d2b-bbd9-792684b242f70.jpg",
        ///                 "LayoutAdvantages":"1、客厅开间4.05米，活动空间充足。\r\n2、南北双卧，互不干扰休息生活。\r\n3、整个房型分割较合理，布局紧凑，功能划分明确。\r\n4、主卧室朝南，冬暖夏凉，采光充足。厨房与主卧相隔较远，减少油烟味。",
        ///                 "LayoutDisadvantages":"1、起居厅的面积标准应大于卧室，\r\n2、厨房/卫生间的面积相应地也应大一些。\r\n3、门多，无稳定间，\r\n4、采光小或采光凹槽深，光线较暗，窗正对墙面，视野差、形状不好或尺度不合理。",
        ///                 "Decotate":[
        ///                     {
        ///                         "DecorateId":39,
        ///                         "DecorateIndex":"/Images/data/201507/dc644352-30ee-483b-8a36-730899918b840.jpg",
        ///                         "DecoratePics":"/Images/data/201507/1cf9bfb9-8c1c-42e8-a4c8-f44e86b0342d0.jpg /Images/data/201507/51bcf0c3-791f-4945-9387-357b9ecb071a1.jpg ",
        ///                         "Decorate360":null
        ///                     },
        ///                     {
        ///                         "DecorateId":40,
        ///                         "DecorateIndex":"/Images/data/201507/e47f9b21-f8d2-43ec-801b-1164710675410.jpg",
        ///                         "DecoratePics":null,
        ///                         "Decorate360":"http://119.188.126.108:8081/ZhuangXiu/DeYang/DeYang.html"
        ///                     }
        ///                 ],
        ///                 "SpotYuanZhuangData":[
        ///                 ],
        ///                 "SpotShiGongData":[
        ///                 ]
        ///             }
        ///         ]
        ///     }
        /// }
        /// 
        /// </returns>
        [HttpPost]
        public JsonResult GetBuildingData(int id)
        {
            Building building = db.Building.Find(id);
            string buildingId = id.ToString();
            UpdateRecord record = db.UpdateRecord.Where(item => item.BuildingId == buildingId).FirstOrDefault();
            if (record == null)
            {
                record = new UpdateRecord();
                record.BuildingId = buildingId;
                record.UpdateTime = DateTime.Now;
                db.UpdateRecord.Add(record);
                db.SaveChanges();
            }
            BuildingData buildingData = new BuildingData();
            buildingData.BuildingId = id;
            buildingData.BuildingName = building.BuildingName;
            buildingData.BuildingIndex = ConfigurationManager.AppSettings["ResourceUrl"] + building.BuildingIndex;
            buildingData.BuildingPics = building.BuildingPics.Substring(0, building.BuildingPics.Length - 1).Split(' ');
            for (int i = 0; i < buildingData.BuildingPics.Length; i++)
            {
                buildingData.BuildingPics[i] = ConfigurationManager.AppSettings["ResourceUrl"] + buildingData.BuildingPics[i];
            }
            buildingData.BuildingIntro = building.BuildingIntro;
            buildingData.BuildingAD = building.BuildingAD;
            buildingData.BuildingAroundPic = ConfigurationManager.AppSettings["ResourceUrl"] + building.BuildingCate;
            buildingData.BuildingDeco = building.BuildingDecorate;
            buildingData.BuildingUpdate = record.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss");
            List<LayoutData> layoutDataList = new List<LayoutData>();

            List<Layout> layoutList = db.Layout.Where(item => item.BuildingId == id).ToList();
            foreach (Layout layout in layoutList)
            {
                List<DecorateData> decorateData = new List<DecorateData>();
                List<SpotYuanZhuangData> spotYuanZhuangData = new List<SpotYuanZhuangData>();
                List<SpotShiGongData> spotShiGongData = new List<SpotShiGongData>();
                LayoutData layoutData = new LayoutData();
                layoutData.LayoutId = layout.Id;
                layoutData.LayoutName = layout.LayoutName;
                layoutData.LayoutPic = ConfigurationManager.AppSettings["ResourceUrl"] + layout.LayoutPic;
                layoutData.LayoutAdvantages = layout.Advantage;
                layoutData.LayoutDisadvantages = layout.Disadvantage;

                List<Decorate> decorateList = db.Decorate.Where(item => item.LayoutId == layout.Id).ToList();
                List<SpotPics> spotPicsList = db.SpotPics.Where(item => item.LayoutId == layout.Id).ToList();
                SpotYuanZhuangData yuanZhuangData = new SpotYuanZhuangData();
                SpotShiGongData shiGongData = new SpotShiGongData();
                foreach (Decorate decorate in decorateList)
                {
                    DecorateData decoratedata = new DecorateData();
                    decoratedata.DecorateId = decorate.Id;
                    decoratedata.DecorateIndex = ConfigurationManager.AppSettings["ResourceUrl"] + decorate.DecorateIndex;
                    if (decoratedata.DecoratePics != null)
                    {
                        decoratedata.DecoratePics = decorate.DecoratePics.Substring(0, decorate.DecoratePics.Length - 1).Split(' ');
                        for (int i = 0; i < decoratedata.DecoratePics.Length; i++)
                        {
                            decoratedata.DecoratePics[i] = ConfigurationManager.AppSettings["ResourceUrl"] + decoratedata.DecoratePics[i];
                        }
                    }
                    else
                    {
                        decoratedata.DecoratePics = new string[] { "" };
                    }
                   
                    decoratedata.Decorate360 = decorate.Decorate360;
                    decorateData.Add(decoratedata);
                }
                foreach (SpotPics spotPic in spotPicsList.Where(item => item.Category == 1))
                {
                    SpotYuanZhuangData yuanzhuangdata = new SpotYuanZhuangData();
                    yuanzhuangdata.SpotId = spotPic.Id;
                    yuanzhuangdata.SpotIndex = ConfigurationManager.AppSettings["ResourceUrl"] + spotPic.SpotIndex;
                    yuanzhuangdata.SpotPics = spotPic.SpotDetails.Substring(0, spotPic.SpotDetails.Length - 1).Split(' ');
                    for (int i = 0; i < yuanzhuangdata.SpotPics.Length; i++)
                    {
                        yuanzhuangdata.SpotPics[i] = ConfigurationManager.AppSettings["ResourceUrl"] + yuanzhuangdata.SpotPics[i];
                    }
                    spotYuanZhuangData.Add(yuanzhuangdata);
                }
                foreach (SpotPics spotPic in spotPicsList.Where(item => item.Category == 2))
                {
                    SpotShiGongData shigongdata = new SpotShiGongData();
                    shigongdata.SpotId = spotPic.Id;
                    shigongdata.SpotIndex = ConfigurationManager.AppSettings["ResourceUrl"] + spotPic.SpotIndex;
                    shigongdata.SpotPics = spotPic.SpotDetails.Substring(0, spotPic.SpotDetails.Length - 1).Split(' ');
                    for (int i = 0; i < shigongdata.SpotPics.Length; i++)
                    {
                        shigongdata.SpotPics[i] = ConfigurationManager.AppSettings["ResourceUrl"] + shigongdata.SpotPics[i];
                    }
                    spotShiGongData.Add(shigongdata);
                }
                layoutData.Decotate = decorateData;
                layoutData.SpotYuanZhuangData = spotYuanZhuangData;
                layoutData.SpotShiGongData = spotShiGongData;

                layoutDataList.Add(layoutData);
            }
            buildingData.LayoutData = layoutDataList;
            return Json(new { data = buildingData }, JsonRequestBehavior.AllowGet);



        }
    }
}