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
    }
}