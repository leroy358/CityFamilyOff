using CityFamily.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityFamily.Controllers
{
    public class FurnitureViewController : Controller
    {
        private CityFamilyDbContext db = new CityFamilyDbContext();
        GetWebSession websession = new GetWebSession();
        int pageSize = 12;
        public ActionResult List(int styleid, int pageIndex = 1)
        {
            if (Session["userName"] != null)
            {
                int adminId = websession.AdminId;
                int companyId = websession.CompanyId;
                //var furniture = db.FurnitureStyle.Where(o => o.StyleId == styleid).OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                //var furniture = db.FurnitureStyle.Where(o => o.StyleId == styleid).Select(o => o.IdentityId).ToList();
                //var pics = db.T_StyleFurniturePics.Where(o => furniture.Contains(o.IdentityId));
                //var furnitureStylePics = pics.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                //int count = pics.Count();
                //InitPage(pageIndex, count, styleid);
                //FurnitureStylePics fsmodel = new FurnitureStylePics();
                //fsmodel.Id = 1;
                //fsmodel.StyleName = (styleid == 1 ? "欧式风格" : (styleid == 2 ? "中式风格" : "现代风格"));
                //fsmodel.T_StyleFurniturePicsList = furnitureStylePics;

                var companyModel = db.T_CompanyInfo.Where(o => o.CompanyID == companyId).FirstOrDefault();
                //var fsmodelAll = db.FurnitureStyle.Where(o => (companyModel.IsFurnitureCoverShield == 1 ? o.CompanyId == companyId : (o.CreateUserId == 1 || o.CompanyId == companyId)) && o.StyleId == styleid).ToList();
                //var fsmodel = db.FurnitureStyle.Where(o => (companyModel.IsFurnitureCoverShield == 1 ? o.CompanyId == companyId : (o.CreateUserId == 1 || o.CompanyId == companyId)) && o.StyleId == styleid).OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();

                var notshowdata = db.FStyleID.Where(o => o.CompanyId == companyId).Select(o => o.FStyleId).ToList();
                var fsmodelAll = db.FurnitureStyle.Where(item => item.StyleId == styleid).Where(o => o.CompanyId == companyId || (o.CreateUserId == 1 && !notshowdata.Contains(o.Id))).ToList();
                var fsmodel = fsmodelAll.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();

                int count = fsmodelAll.Count();
                InitPage(pageIndex, count, styleid);
                return View(fsmodel);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        private void InitPage(int pageIndex, int count, int styleid)
        {
            int pageCount = (count % pageSize == 0) ? count / pageSize : count / pageSize + 1;
            if (pageCount == 0)
            {
                pageCount = 1;
            }
            string perPage = Url.Action("List", new { styleid, pageIndex = (pageIndex < 2) ? 1 : pageIndex - 1 });
            string nextPage = Url.Action("List", new { styleid, pageIndex = (pageIndex == pageCount) ? pageCount : (pageIndex + 1) });
            string lastPage = Url.Action("List", new { styleid, pageIndex = pageCount });
            string firstPage = Url.Action("List", new { styleid, pageIndex = 1 });
            string pageX = Url.Action("List", new { styleid, pageIndex });
            ViewBag.perPage = perPage;
            ViewBag.nextPage = nextPage;
            ViewBag.pageCount = pageCount;
            ViewBag.pageX = pageX.Substring(pageX.Length - 1, 1);
            ViewBag.lastPage = lastPage;
            ViewBag.firstPage = firstPage;
        }


        public ActionResult CoverList()
        {
            if (Session["userName"] != null)
            {
                int adminId = websession.AdminId;
                int companyId = websession.CompanyId;
                //var companyModel = db.T_CompanyInfo.Where(o => o.CompanyID == companyId).FirstOrDefault();
                var notshowdata = db.FCoverID.Where(o => o.CompanyId == companyId).Select(o => o.FCoverId).ToList();
                List<T_FurnitureCover> list = db.T_FurnitureCover.Where(o => o.CompanyId == companyId || (o.CreateUserId == 1 && !notshowdata.Contains(o.Id))).ToList();
                return View(list);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult Detail(int id)
        {
            if (Session["userName"] != null)
            {
                var furnitureStylePics = db.FurnitureStyle.Find(id);
                //int styleid = furnitureStylePics.StyleId;
                //FurnitureStylePics fsmodel = new FurnitureStylePics();
                //fsmodel.StyleId = styleid;
                //fsmodel.StyleName = (styleid == 1 ? "欧式风格" : (styleid == 2 ? "中式风格" : "现代风格"));
                //fsmodel.BackPic = db.FurnitureStyle.Where(o => o.StyleId == styleid).FirstOrDefault().BackPic;
                //fsmodel.T_StyleFurniturePicsList = db.T_StyleFurniturePics.Where(o => o.StyleId == styleid).ToList();
                //fsmodel.Id = id;
                return View(furnitureStylePics);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        public ActionResult SaveSessionPic(string pic, int Id, int ModelId)
        {
            if (Session["userName"] != null)
            {
                if (Session["FurnitureViewUrl"] != null)
                {
                    string pics = Session["FurnitureViewUrl"].ToString();
                    if (pics.Contains(pic))
                    {
                        Session["FurnitureViewUrl"] = pics;
                    }
                    else
                    {
                        Session["FurnitureViewUrl"] = pics + "&" + pic;
                    }
                }
                else
                {
                    Session["FurnitureViewUrl"] = pic;
                }
                return RedirectToAction("Detail", new { id = ModelId });
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        /// <summary>
        /// 获取家具场景封面列表
        /// </summary>
        /// <param name="companyId">登录用户分公司ID</param>
        /// <returns>
        /// {
        ///     "data":[
        ///         {
        ///             "FurnitureId":20,
        ///             "FurnitureName":"华丽·高贵·浪漫",
        ///             "FurnitureIndex":"/Images/data/201512/d5309c85-a612-45af-b89d-1d3ad0d0b7a10.jpg"
        ///         },
        ///         {
        ///             "FurnitureId":21,
        ///             "FurnitureName":"简洁·现代·时尚",
        ///             "FurnitureIndex":"/Images/data/201512/200b882c-3d1b-40f9-b527-dcd40da2885a0.jpg"
        ///         }
        ///     ]
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult GetFurnitureCover(int companyId)
        {
            var notshowdata = db.FCoverID.Where(o => o.CompanyId == companyId).Select(o => o.FCoverId).ToList();
            List<T_FurnitureCover> list = db.T_FurnitureCover.Where(o => o.CompanyId == companyId || (o.CreateUserId == 1 && !notshowdata.Contains(o.Id))).ToList();
            List<T_FurnitureCoverList> furnitureList = new List<T_FurnitureCoverList>();
            foreach (T_FurnitureCover furniture in list)
            {
                T_FurnitureCoverList furnitureCover = new T_FurnitureCoverList();
                furnitureCover.FurnitureId = furniture.Id;
                furnitureCover.FurnitureName = furniture.StyleName;
                furnitureCover.FurnitureIndex = ConfigurationManager.AppSettings["ResourceUrl"] + furniture.StylePic;

                string id = furniture.Id.ToString();
                UpdateRecord record = db.UpdateRecord.Where(item => item.FurnitureId == id ).FirstOrDefault();
                if (record == null)
                {
                    record = new UpdateRecord();
                    record.FurnitureId = id;
                    record.UpdateTime = DateTime.Now;
                    db.UpdateRecord.Add(record);
                    db.SaveChanges();
                }

                furnitureCover.UpdateTime = record.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss");
                furnitureList.Add(furnitureCover);
            }
            return Json(new { data = furnitureList }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取家具场景二级封面
        /// </summary>
        /// <param name="companyId">登录用户分公司ID</param>
        /// <param name="styleId">家具场景一级分类ID</param>
        /// <returns>
        /// {
        ///     "data":[
        ///         {
        ///             "FurnitureId":178,
        ///             "FurniturePic":"/Images/data/201512/8c18f02e-03e8-4ea0-9c9b-63b1fc84f8400.jpg"
        ///         },
        ///         {
        ///             "FurnitureId":177,
        ///             "FurniturePic":"/Images/data/201512/7b13c4f5-4369-4059-bc27-603e93b39c900.jpg"
        ///         }
        ///     ]
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult GetFurnitureIndex(int companyId, int styleId)
        {
            var notshowdata = db.FStyleID.Where(o => o.CompanyId == companyId).Select(o => o.FStyleId).ToList();
            //var fsmodelAll = db.FurnitureStyle.Where(item => item.StyleId == styleId && item.CompanyId == companyId || (item.CreateUserId == 1 && !notshowdata.Contains(item.Id))).OrderByDescending(item => item.Id).Take(12).ToList();
            var fsmodelAll = db.FurnitureStyle.Where(item => item.StyleId == styleId && (item.CompanyId == companyId || (item.CreateUserId == 1 && !notshowdata.Contains(item.Id)))).OrderByDescending(item => item.Id).Take(12).ToList();
            T_FurnitureCover cover = db.T_FurnitureCover.Find(styleId); 
            List<FurnitureStyleList> furnitureStyleList = new List<FurnitureStyleList>();
            foreach (FurnitureStyle furnitureStyle in fsmodelAll)
            {
                FurnitureStyleList furniturestyleList = new FurnitureStyleList();
                furniturestyleList.FurnitureCoverName = cover.StyleName;
                furniturestyleList.FurnitureId = furnitureStyle.Id;
                furniturestyleList.FurniturePic = ConfigurationManager.AppSettings["ResourceUrl"] + furnitureStyle.IndexPic;
                furnitureStyleList.Add(furniturestyleList);
            }
            return Json(new { data = furnitureStyleList }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="styleId"></param>
        /// <param name="furnitureId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetFurnitureIndexNext(int companyId, int styleId, int furnitureId)
        {
            var notshowdata = db.FStyleID.Where(o => o.CompanyId == companyId).Select(o => o.FStyleId).ToList();
            var fsmodelAll = db.FurnitureStyle.OrderByDescending(item => item.Id).Where(item => item.Id < furnitureId && (item.StyleId == styleId && (item.CompanyId == companyId || (item.CreateUserId == 1 && !notshowdata.Contains(item.Id))))).Take(12);
            List<FurnitureStyleList> furnitureStyleList = new List<FurnitureStyleList>();
            foreach (FurnitureStyle furnitureStyle in fsmodelAll)
            {
                FurnitureStyleList furniturestyleList = new FurnitureStyleList();
                furniturestyleList.FurnitureId = furnitureStyle.Id;
                furniturestyleList.FurniturePic = ConfigurationManager.AppSettings["ResourceUrl"] + furnitureStyle.IndexPic;
                furnitureStyleList.Add(furniturestyleList);
            }
            return Json(new { data = furnitureStyleList }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取家具场景详情
        /// </summary>
        /// <param name="furnitureId"></param>
        /// <returns>
        /// {
        ///     "data":{
        ///         "FurnitureId":170,
        ///         "FurnitureBrand":"瑞名华高档定制餐桌",
        ///         "FurnitureSize":"餐桌【1600*850*750】 餐椅【480*440*940】 ",
        ///         "FurniturePrize":"实木框架+布艺",
        ///         "FurnitureMaterial":"组合【26000元】",
        ///         "FurniturePic":"/Images/data/201512/a0638776-f92a-4678-b7d8-4f67600dc6870.jpg"
        ///     }
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult GetFurnitureDetails(int furnitureId)
        {
            FurnitureStyle furnitureStyle = db.FurnitureStyle.Find(furnitureId);
            FurnitureStyleDetails furnitureDetail = new FurnitureStyleDetails();
            furnitureDetail.FurnitureId = furnitureStyle.Id;

            string[] args = furnitureStyle.StyleName.Split('&').ToArray();

            furnitureDetail.FurnitureBrand = args[0];
            furnitureDetail.FurnitureSize = args[1];
            furnitureDetail.FurniturePrize = args[2];
            furnitureDetail.FurnitureMaterial = args[3];

            furnitureDetail.FurniturePic = ConfigurationManager.AppSettings["ResourceUrl"] + furnitureStyle.FurniturePics;
            return Json(new { data = furnitureDetail }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 判断家具场景是否有更新
        /// </summary>
        /// <param name="styleId">家具封面ID</param>
        /// <param name="updateTime">家具场景更新时间</param>
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
        public ActionResult IsFurnitureUpdate(int styleId, string updateTime)
        {
            DateTime time = Convert.ToDateTime(updateTime);
            string id = styleId.ToString();
            UpdateRecord record = db.UpdateRecord.Where(item => item.StyleId == id).FirstOrDefault();
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
        /// 
        /// </summary>
        /// <param name="styleId">登录用户分公司ID</param>
        /// <param name="companyId">家具封面ID</param>
        /// <returns>  
        /// {
        ///     "data":
        ///     {
        ///         "FurnitureCoverId":20,
        ///         "FurnitureCoverName":"华丽·高贵·浪漫",
        ///         "FurnitureCoverIndex":"http://119.188.113.104:8081/Images/data/201512/d5309c85-a612-45af-b89d-1d3ad0d0b7a10.jpg",
        ///         "FurnitureCoverUpdate":"2016-04-20 10:19:30",
        ///         "FurnitureStyleData":[
        ///             {
        ///                 "FurnitureId":178,
        ///                 "FurnitureIndexPic":"http://119.188.113.104:8081/Images/data/201512/a44d8fac-c128-4688-a6fe-2b22b323d7420.jpg",
        ///                 "FurniturePic":"http://119.188.113.104:8081/Images/data/201512/8c18f02e-03e8-4ea0-9c9b-63b1fc84f8400.jpg",
        ///                 "FurnitureBrand":"筑家家具高档定制餐桌",
        ///                 "FurnitureSize":"餐桌【直径1600】 餐椅【480*440*840】","FurniturePrize":"餐桌【6500元】 餐椅【1800元】"
        ///                 "FurnitureMaterial":"实木框架+布艺",
        ///                 "FurnitureUpdate":null
        ///             },
        ///             {
        ///                 "FurnitureId":168,
        ///                 "FurnitureIndexPic":"http://119.188.113.104:8081/Images/data/201512/8b22c49e-21a3-4379-971f-d904c24aedd60.jpg",
        ///                 "FurniturePic":"http://119.188.113.104:8081/Images/data/201512/97cacffd-1cd4-472d-bcb2-9d73869417890.jpg",
        ///                 "FurnitureBrand":"林氏木业高档定制沙发","
        ///                 FurnitureSize":"餐桌【1500*850*750】 餐椅【480*440*740】",
        ///                 "FurniturePrize":"组合【13500元】",
        ///                 "FurnitureMaterial":"实木框架+布艺",
        ///                 "FurnitureUpdate":null
        ///            },
        ///            {
        ///                 "FurnitureId":167,
        ///                 "FurnitureIndexPic":"http://119.188.113.104:8081/Images/data/201512/72d119c5-b709-42ad-a0fb-1427f2029a9f0.jpg",
        ///                 "FurniturePic":"http://119.188.113.104:8081/Images/data/201512/429097e7-607a-4441-94b2-500a027989180.jpg",
        ///                 "FurnitureBrand":"林氏木业高档定制沙发",
        ///                 "FurnitureSize":"单人【850*800*900】 三人【2350*950*900】",
        ///                 "FurniturePrize":"组合【26500元】",
        ///                 "FurnitureMaterial":"实木框架+布艺",
        ///                 "FurnitureUpdate":null
        ///             }
        ///         ]
        ///     }
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult GetFurnitureAll(int styleId,int companyId)
        {
            string id = styleId.ToString();
            UpdateRecord record = db.UpdateRecord.Where(item => item.FurnitureId == id).FirstOrDefault();
            if (record == null)
            {
                record = new UpdateRecord();
                record.FurnitureId = id;
                record.UpdateTime = DateTime.Now;
                db.UpdateRecord.Add(record);
                db.SaveChanges();
            }
            T_FurnitureCover furnitureCover = db.T_FurnitureCover.Find(styleId);
            var notshowdata = db.FStyleID.Where(o => o.CompanyId == companyId).Select(o => o.FStyleId).ToList();
            var fsmodelAll = db.FurnitureStyle.Where(item => item.StyleId == styleId && (item.CompanyId == companyId || (item.CreateUserId == 1 && !notshowdata.Contains(item.Id)))).OrderByDescending(item => item.Id).ToList();
            T_FurnitureCoverData furnitureCoverData = new T_FurnitureCoverData();
            furnitureCoverData.FurnitureCoverId = furnitureCover.Id;
            furnitureCoverData.FurnitureCoverName = furnitureCover.StyleName;
            furnitureCoverData.FurnitureCoverIndex = ConfigurationManager.AppSettings["ResourceUrl"] + furnitureCover.StylePic;
            furnitureCoverData.FurnitureCoverUpdate = record.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss");
            List<FurnitureStyleData> furnitureStyleList = new List<FurnitureStyleData>();
            foreach (FurnitureStyle furnitureStyle in fsmodelAll)
            {
                FurnitureStyleData furnitureStyleData = new FurnitureStyleData();
                furnitureStyleData.FurnitureId = furnitureStyle.Id;
                furnitureStyleData.FurnitureIndexPic = ConfigurationManager.AppSettings["ResourceUrl"] + furnitureStyle.IndexPic;
                furnitureStyleData.FurniturePic = ConfigurationManager.AppSettings["ResourceUrl"] + furnitureStyle.FurniturePics;

                string[] args = furnitureStyle.StyleName.Split('&').ToArray();

                furnitureStyleData.FurnitureBrand = args[0];
                furnitureStyleData.FurnitureSize = args[1];
                furnitureStyleData.FurniturePrize = args[2];
                furnitureStyleData.FurnitureMaterial = args[3];

                furnitureStyleList.Add(furnitureStyleData);
            }
            furnitureCoverData.FurnitureStyleData = furnitureStyleList;

            return Json(new { data = furnitureCoverData }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 据客户端最近更新时间判断服务器端是否有更新，获取一类家具场景下所有数据
        /// </summary>
        /// <param name="styleId">家具场景ID</param>
        /// <returns>
        /// {
        ///     "data":
        ///     {
        ///         "FurnitureId":177,
        ///         "FurnitureIndexPic":"http://119.188.113.104:8081/Images/data/201512/c31acaf5-cfac-4813-894e-4c37d5dd8bda0.jpg",
        ///         "FurniturePic":"http://119.188.113.104:8081/Images/data/201512/7b13c4f5-4369-4059-bc27-603e93b39c900.jpg",
        ///         "FurnitureBrand":"筑家家具高档定制沙发",
        ///         "FurnitureSize":"贵妃榻【1650*870*800】 三人【2350*880*800】",
        ///         "FurniturePrize":"贵妃榻【8500元】 三人【12600元】",
        ///         "FurnitureMaterial":"实木框架+布艺",
        ///                                                                                      "FurnitureUpdate":"2016-03-25 20:38:01"
        ///     }
        /// }
        /// </returns>
        [HttpPost]
        public ActionResult GetFurnitureData(int styleId)
        {
            string id = styleId.ToString();
            //UpdateRecord record = db.UpdateRecord.Where(item => item.FurnitureId == id).FirstOrDefault();
            //if (record == null)
            //{
            //    record = new UpdateRecord();
            //    record.FurnitureId = id;
            //    record.UpdateTime = DateTime.Now;
            //    db.UpdateRecord.Add(record);
            //    db.SaveChanges();
            //}
            FurnitureStyleData furnitureStyleData = new FurnitureStyleData();
            FurnitureStyle furniture = db.FurnitureStyle.Find(styleId);
            furnitureStyleData.FurnitureId = furniture.Id;
            furnitureStyleData.FurnitureIndexPic = ConfigurationManager.AppSettings["ResourceUrl"] + furniture.IndexPic;
            furnitureStyleData.FurniturePic = ConfigurationManager.AppSettings["ResourceUrl"] + furniture.FurniturePics;
            string[] args = furniture.StyleName.Split('&').ToArray();

            furnitureStyleData.FurnitureBrand = args[0];
            furnitureStyleData.FurnitureSize = args[1];
            furnitureStyleData.FurniturePrize = args[2];
            furnitureStyleData.FurnitureMaterial = args[3];
            //furnitureStyleData.FurnitureUpdate = record.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss");

            int coverId = furniture.StyleId;
            int companyId = furniture.CompanyId;
            var notshowdata = db.FStyleID.Where(o => o.CompanyId == companyId).Select(o => o.FStyleId).ToList();
            FurnitureStyle preStyle = db.FurnitureStyle.OrderByDescending(item => item.Id).Where(item => item.Id > styleId && (item.StyleId == coverId && (item.CompanyId == companyId || (item.CreateUserId == 1 && !notshowdata.Contains(item.Id))))).OrderBy(item => item.Id).FirstOrDefault();
            int pre = 0;
            if (preStyle != null)
            {
                pre = 1;
            }

            int next = 0;
            FurnitureStyle nextStyle = db.FurnitureStyle.OrderByDescending(item => item.Id).Where(item => item.Id < styleId && (item.StyleId == coverId && (item.CompanyId == companyId || (item.CreateUserId == 1 && !notshowdata.Contains(item.Id))))).FirstOrDefault();
            if (nextStyle != null)
            {
                next = 1;
            }
            return Json(new { pre = pre, data = furnitureStyleData, next = next }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetScrollFurnitureDetail(int furnitureId, string action, int companyId)
        {
            FurnitureStyle furnitureStyle = db.FurnitureStyle.Find(furnitureId);
            int coverId = furnitureStyle.StyleId;
            var notshowdata = db.FStyleID.Where(o => o.CompanyId == companyId).Select(o => o.FStyleId).ToList();
            if (action == "pre")
            {
                FurnitureStyle preStyle = db.FurnitureStyle.OrderByDescending(item => item.Id).Where(item => item.Id > furnitureId && (item.StyleId == coverId && (item.CompanyId == companyId || (item.CreateUserId == 1 && !notshowdata.Contains(item.Id))))).OrderBy(item=>item.Id).FirstOrDefault();
                if (preStyle != null)
                {
                    FurnitureStyleDetails furnitureDetail = new FurnitureStyleDetails();
                    furnitureDetail.FurnitureId = preStyle.Id;

                    string[] args = preStyle.StyleName.Split('&').ToArray();

                    furnitureDetail.FurnitureBrand = args[0];
                    furnitureDetail.FurnitureSize = args[1];
                    furnitureDetail.FurniturePrize = args[2];
                    furnitureDetail.FurnitureMaterial = args[3];
                    furnitureDetail.FurniturePic = ConfigurationManager.AppSettings["ResourceUrl"] + preStyle.FurniturePics;

                    int preId = preStyle.Id;
                    FurnitureStyle prepreStyle = db.FurnitureStyle.OrderByDescending(item => item.Id).Where(item => item.Id > preId && (item.StyleId == coverId && (item.CompanyId == companyId || (item.CreateUserId == 1 && !notshowdata.Contains(item.Id))))).OrderBy(item => item.Id).FirstOrDefault();
                    int pre = 0;
                    if (prepreStyle != null)
                    {
                        pre = 1;
                    }

                    //FurnitureStyle nextStyle = db.FurnitureStyle.OrderByDescending(item => item.Id).Where(item => item.Id < preId && (item.StyleId == coverId && (item.CompanyId == companyId || (item.CreateUserId == 1 && !notshowdata.Contains(item.Id))))).OrderBy(item => item.Id).FirstOrDefault();
                    //int next = 0;
                    //if (nextStyle != null)
                    //{
                    //    next = 1;
                    //}

                    return Json(new { pre = pre, data = furnitureDetail, next = 1 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { pre = 0, data = "", next = 1 }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                FurnitureStyle nextStyle = db.FurnitureStyle.OrderByDescending(item => item.Id).Where(item => item.Id < furnitureId && (item.StyleId == coverId && (item.CompanyId == companyId || (item.CreateUserId == 1 && !notshowdata.Contains(item.Id))))).FirstOrDefault();
                if (nextStyle != null)
                {
                    FurnitureStyleDetails furnitureDetail = new FurnitureStyleDetails();
                    furnitureDetail.FurnitureId = nextStyle.Id;

                    string[] args = nextStyle.StyleName.Split('&').ToArray();

                    furnitureDetail.FurnitureBrand = args[0];
                    furnitureDetail.FurnitureSize = args[1];
                    furnitureDetail.FurniturePrize = args[2];
                    furnitureDetail.FurnitureMaterial = args[3];
                    furnitureDetail.FurniturePic = ConfigurationManager.AppSettings["ResourceUrl"] + nextStyle.FurniturePics;

                    int nextId = nextStyle.Id;
                    //FurnitureStyle preStyle = db.FurnitureStyle.OrderByDescending(item => item.Id).Where(item => item.Id > nextId && (item.StyleId == coverId && (item.CompanyId == companyId || (item.CreateUserId == 1 && !notshowdata.Contains(item.Id))))).OrderBy(item => item.Id).FirstOrDefault();
                    //int pre = 0;
                    //if (preStyle != null)
                    //{
                    //    pre = 1;
                    //}

                    FurnitureStyle nextnextStyle = db.FurnitureStyle.OrderByDescending(item => item.Id).Where(item => item.Id < nextId && (item.StyleId == coverId && (item.CompanyId == companyId || (item.CreateUserId == 1 && !notshowdata.Contains(item.Id))))).OrderBy(item => item.Id).FirstOrDefault();
                    int next = 0;
                    if (nextnextStyle != null)
                    {
                        next = 1;
                    }

                    return Json(new { pre = 1, data = furnitureDetail, next = next }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { pre = 1, data = "", next = 0 }, JsonRequestBehavior.AllowGet);
                }
            }

            //FurnitureStyleDetails furnitureDetail = new FurnitureStyleDetails();
            //furnitureDetail.FurnitureId = furnitureStyle.Id;

            //string[] args = furnitureStyle.StyleName.Split('&').ToArray();

            //furnitureDetail.FurnitureBrand = args[0];
            //furnitureDetail.FurnitureSize = args[1];
            //furnitureDetail.FurniturePrize = args[2];
            //furnitureDetail.FurnitureMaterial = args[3];
            //furnitureDetail.FurniturePic = ConfigurationManager.AppSettings["ResourceUrl"] + furnitureStyle.FurniturePics;


            //int coverId = furnitureStyle.StyleId;
            //var notshowdata = db.FStyleID.Where(o => o.CompanyId == companyId).Select(o => o.FStyleId).ToList();
            //FurnitureStyle nextStyle = db.FurnitureStyle.OrderByDescending(item => item.Id).Where(item => item.Id < furnitureId && (item.StyleId == coverId && (item.CompanyId == companyId || (item.CreateUserId == 1 && !notshowdata.Contains(item.Id))))).FirstOrDefault();
            

            

        }

    }
}