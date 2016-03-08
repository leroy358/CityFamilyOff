using CityFamily.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityFamily.Controllers
{
    public class LayoutsController : Controller
    {
        private CityFamilyDbContext db = new CityFamilyDbContext();
        public ActionResult LayoutDetails(int id)
        {
            if (Session["userName"] != null)
            {
                Layout layout = db.Layout.Find(id);
                Building building = db.Building.Where(item => item.Id == layout.BuildingId).FirstOrDefault();
                ViewBag.BuildName = building.BuildingName;
                ViewBag.BuildId = building.Id;
                var decorates = db.Decorate.Where(item => item.LayoutId == id);
                var spotPics = db.SpotPics.Where(item => item.LayoutId == id);
                LayoutDecorateView view = new LayoutDecorateView();
                view.layout = layout;
                view.decorates = decorates.ToList();
                view.spotPics = spotPics.ToList();
                return View(view);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
            
        }
        //public void DownLoad(string filePath)
        //{
        //    if (Session["userName"] != null)
        //    {
        //        //还原文件路径
        //        filePath = "/CADFiles/data/" + filePath;

        //        //获取文件大小
        //        filePath = Server.MapPath(filePath);
        //        FileInfo info = new FileInfo(filePath);
        //        long fileSize = info.Length;

        //        //设置下载文件名
        //        int intStart = filePath.LastIndexOf("\\") + 1;
        //        string saveFileName = filePath.Substring(intStart, filePath.Length - intStart);
        //        HttpContext.Response.Clear();

        //        //指定Http Mime格式为压缩包
        //        HttpContext.Response.ContentType = "application/x-zip-compressed";

        //        // Http 协议中有专门的指令来告知浏览器, 本次响应的是一个需要下载的文件. 格式如下:
        //        // Content-Disposition: attachment;filename=filename.ext
        //        HttpContext.Response.AddHeader("Content-Disposition", "attachment;filename=" + saveFileName);

        //        //不指明Content-Length用Flush的话不会显示下载进度   
        //        HttpContext.Response.AddHeader("Content-Length", fileSize.ToString());
        //        HttpContext.Response.TransmitFile(filePath, 0, fileSize);
        //        HttpContext.Response.Flush();
        //        HttpContext.Response.Close();
        //    }            
        //}
        public ActionResult DownLoad(string filePath) 
        {
            if (Session["userName"] != null)
            {
                //还原文件路径
                filePath = "/CADFiles/data/" + filePath;

                filePath = Server.MapPath(filePath);

                //设置下载文件名
                int intStart = filePath.LastIndexOf("\\") + 1;
                string saveFileName = filePath.Substring(intStart, filePath.Length - intStart);

                return File(filePath, "application/rar", saveFileName);

            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        /// <summary>
        /// 获取户型图详情页信息
        /// </summary>
        /// <param name="layoutId">户型图ID</param>
        /// <returns>
        /// {
        ///     "data":{
        ///         "LayoutId":16,
        ///         "LayoutName":"轩苑紫郡108㎡",
        ///         "LayoutPic":"/Images/data/201507/0d74c585-f32b-4cff-9774-3a9bec8b4e400.jpg",
        ///         "LayoutAdvantages":"1、客厅开间4.05米，活动空间充足。\r\n2、南北双卧，互不干扰休息生活。\r\n3、整个房型分割较合理，布局紧凑，功能划分明确。\r\n4、主卧室朝南，冬暖夏凉，采光充足。厨房与主卧相隔较远，减少油烟味。",
        ///         "LayoutDisadvantages":"1、起居厅的面积标准应大于卧室，\r\n2、厨房/卫生间的面积相应地也应大一些。\r\n3、门多，无稳定间，\r\n4、采光小或采光凹槽深，光线较暗，窗正对墙面，视野差、形状不好或尺度不合理。",
        ///         "Decotate":[
        ///             {
        ///                 "DecorateId":35,
        ///                 "DecorateIndex":"/Images/data/201507/d302d0f6-b9f6-47f1-9141-f49a8c7c7cf30.jpg",
        ///                 "DecoratePics":"/Images/data/201507/b480a31d-3f82-4903-85c5-11f0f2a7a2aa0.jpg /Images/data/201507/28c599a0-d4b2-4c82-870f-28877c2455e41.jpg ",
        ///                 "Decorate360":null
        ///             },
        ///             {
        ///                 "DecorateId":41,
        ///                 "DecorateIndex":"/Images/data/201507/53e13946-4f37-488f-bc4c-0ba8449115390.jpg",
        ///                 "DecoratePics":null,
        ///                 "Decorate360":"http://119.188.126.108:8081/ZhuangXiu/BaoJi/BaoJi.html"
        ///             }
        ///         ],
        ///         "SpotYuanZhuangData":[
        ///             {
        ///                 "SpotId":17,
        ///                 "SpotIndex":"/Images/data/201507/d2f5739b-38a6-4e2a-8982-2ccd88ef2bd20.jpg",
        ///                 "SpotPics":"/Images/data/201507/6ab2fcb9-8762-4ee6-8e23-8b912ec428cc0.jpg /Images/data/201507/723346ec-1d15-4e50-9ee4-b69dc062cccc1.png "
        ///             },
        ///             {
        ///                 "SpotId":43,
        ///                 "SpotIndex":"/Images/data/201507/541f73f8-c0d1-4110-804a-3ec5c2c2e9c70.png",
        ///                 "SpotPics":"/Images/data/201507/37c51b81-4f32-4f14-b026-ad94c4c552930.png "
        ///             }
        ///         ],
        ///         "SpotShiGongData":[
        ///             {
        ///                 "SpotId":18,
        ///                 "SpotIndex":"/Images/data/201507/541f73f8-c0d1-4110-804a-3ec5c2c2e9c70.png",
        ///                 "SpotPics":"/Images/data/201507/37c51b81-4f32-4f14-b026-ad94c4c552930.png "
        ///             },
        ///             {
        ///                 "SpotId":42,
        ///                 "SpotIndex":"/Images/data/201507/d2f5739b-38a6-4e2a-8982-2ccd88ef2bd20.jpg",
        ///                 "SpotPics":"/Images/data/201507/6ab2fcb9-8762-4ee6-8e23-8b912ec428cc0.jpg /Images/data/201507/723346ec-1d15-4e50-9ee4-b69dc062cccc1.png "
        ///             }
        ///         ]
        ///     }
        /// }
        /// </returns>
        public ActionResult GetLayoutDetails(int layoutId)
        {
            Layout layout = db.Layout.Find(layoutId);
            List<LayoutDecorate> decorateData = new List<LayoutDecorate>();
            List<LayoutSpotYuanZhuang> spotYuanZhuangData = new List<LayoutSpotYuanZhuang>();
            List<LayoutSpotShiGong> spotShiGongData = new List<LayoutSpotShiGong>();
            LayoutDetails layoutData = new LayoutDetails();
            layoutData.LayoutId = layout.Id;
            layoutData.LayoutName = layout.LayoutName;
            layoutData.LayoutPic = layout.LayoutPic;
            layoutData.LayoutAdvantages = layout.Advantage;
            layoutData.LayoutDisadvantages = layout.Disadvantage;

            List<Decorate> decorateList = db.Decorate.Where(item => item.LayoutId == layout.Id).ToList();
            List<SpotPics> spotPicsList = db.SpotPics.Where(item => item.LayoutId == layout.Id).ToList();
            SpotYuanZhuangData yuanZhuangData = new SpotYuanZhuangData();
            SpotShiGongData shiGongData = new SpotShiGongData();
            foreach (Decorate decorate in decorateList)
            {
                LayoutDecorate decoratedata = new LayoutDecorate();
                decoratedata.DecorateId = decorate.Id;
                decoratedata.DecorateIndex = decorate.DecorateIndex;
                decoratedata.DecoratePics = decorate.DecoratePics;
                decoratedata.Decorate360 = decorate.Decorate360;
                decorateData.Add(decoratedata);
            }
            foreach (SpotPics spotPic in spotPicsList.Where(item => item.Category == 1))
            {
                LayoutSpotYuanZhuang yuanzhuangdata = new LayoutSpotYuanZhuang();
                yuanzhuangdata.SpotId = spotPic.Id;
                yuanzhuangdata.SpotIndex = spotPic.SpotIndex;
                yuanzhuangdata.SpotPics = spotPic.SpotDetails;
                spotYuanZhuangData.Add(yuanzhuangdata);
            }
            foreach (SpotPics spotPic in spotPicsList.Where(item => item.Category == 2))
            {
                LayoutSpotShiGong shigongdata = new LayoutSpotShiGong();
                shigongdata.SpotId = spotPic.Id;
                shigongdata.SpotIndex = spotPic.SpotIndex;
                shigongdata.SpotPics = spotPic.SpotDetails;
                spotShiGongData.Add(shigongdata);
            }
            layoutData.Decotate = decorateData;
            layoutData.SpotYuanZhuangData = spotYuanZhuangData;
            layoutData.SpotShiGongData = spotShiGongData;
            return Json(new { data = layoutData }, JsonRequestBehavior.AllowGet);

        }
    }
}