using CityFamily.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CityFamily.Areas.Admin.Controllers
{
    public class SpaceCateController : Controller
    {
        private CityFamilyDbContext db = new CityFamilyDbContext();
        public ActionResult Edit(int category)
        {
            var pics = db.SpaceCate.Where(item => item.Category == category).FirstOrDefault();
            ViewBag.IsCreate = (pics != null) ? true : false;
            ViewBag.Category = category;
            return View(pics);
        }
        [HttpPost]
        public ActionResult SaveEdit(SpaceCate spaceCate,bool IsCreate)
        {

            //string[] pics = spaceCate.SpacePics.Substring(0, spaceCate.SpacePics.Length - 1).Split(' ');
            //StringBuilder sb=new StringBuilder();
            //foreach (var pic in pics)
            //{
            //    sb.Append(ToSmall(pic));
            //    sb.Append(" ");
            //}
            //spaceCate.SpacePics = sb.ToString();
            if (IsCreate)
            {
                db.Entry(spaceCate).State = EntityState.Modified;
            }
            else
            {
                db.SpaceCate.Add(spaceCate);
            }
            db.SaveChanges();
            return RedirectToAction("Edit", new { category = spaceCate.Category });
        }
        //public string ToSmall(string originalImagePath)
        //{
        //    bool isExist = WXSSK.Common.DirectoryAndFile.FileExists(originalImagePath);
        //    System.Drawing.Image imgOriginal = System.Drawing.Image.FromFile(Server.MapPath(originalImagePath));

        //    //获取原图片的的宽度与高度
        //    int originalWidth = imgOriginal.Width;
        //    int originalHeight = imgOriginal.Height;

        //    //originalHeight = (int)originalHeight * intWidth / originalWidth;  //高度做相应比例缩小
        //    //originalWidth = intWidth;  //宽度等于缩略图片尺寸

        //    System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(originalWidth, originalHeight);
        //    System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);

        //    //设置缩略图片质量
        //    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
        //    graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
        //    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

        //    graphics.DrawImage(imgOriginal, 0, 0, originalWidth, originalHeight);

        //    // 保存缩略图片
        //    imgOriginal.Dispose();
        //    WXSSK.Common.DirectoryAndFile.DeleteFile(originalImagePath);
        //    bitmap.Save(Server.MapPath(originalImagePath), System.Drawing.Imaging.ImageFormat.Jpeg);
        //    return originalImagePath;
        //}
	}
}