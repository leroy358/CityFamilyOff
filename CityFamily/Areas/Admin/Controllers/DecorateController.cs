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
    public class DecorateController : Controller
    {
        private CityFamilyDbContext db = new CityFamilyDbContext();

        public ActionResult List(int id)
        {
            if (Session["admin"] != null)
            {
                var decorates = db.Decorate.Where(item => item.LayoutId == id).OrderByDescending(item => item.Id);
                ViewBag.LayId = id;
                Layout layout = db.Layout.Find(id);
                ViewBag.LayName = layout.LayoutName;
                Building build = db.Building.Find(layout.BuildingId);
                ViewBag.BuildName = build.BuildingName;
                ViewBag.BuildId = build.Id;
                return View(decorates);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
            
        }
        public ActionResult Create(int id)
        {
            if (Session["admin"] != null)
            {
                ViewBag.IsCreate = false;
                Decorate decorate = new Decorate();
                decorate.LayoutId = id;
                Layout layout = db.Layout.Find(id);
                ViewBag.LayName = layout.LayoutName;
                ViewBag.LayId = id;
                Building build = db.Building.Find(layout.BuildingId);
                ViewBag.BuildId = build.Id;
                ViewBag.BuildName = build.BuildingName;
                return View("Edit", decorate);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        public ActionResult Edit(int id)
        {
            if (Session["admin"] != null)
            {
                Decorate decorate = db.Decorate.Find(id);
                Layout layout = db.Layout.Find(decorate.LayoutId);
                ViewBag.LayId = layout.Id;
                ViewBag.LayName = layout.LayoutName;
                Building build = db.Building.Find(layout.BuildingId);
                ViewBag.BuildId = build.Id;
                ViewBag.BuildName = build.BuildingName;
                ViewBag.IsCreate = true;
                return View(decorate);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        [HttpPost]
        public ActionResult SavEdit(Decorate decorate, bool IsCreate)
        {
            if (Session["admin"] != null)
            {
                decorate.CreateTime = DateTime.Now.ToString("yyyy-MM-dd");
                decorate.DecorateIndex = ToSmall(decorate.DecorateIndex);
                if (decorate.DecoratePics != null)
                {
                    string[] pics = decorate.DecoratePics.Substring(0, decorate.DecoratePics.Length - 1).Split(' ');
                    StringBuilder sb = new StringBuilder();
                    foreach (var pic in pics)
                    {
                        sb.Append(ToSmall(pic));
                        sb.Append(" ");
                    }
                    decorate.DecoratePics = sb.ToString();
                }                
                if (IsCreate)
                {
                    db.Entry(decorate).State = EntityState.Modified;
                }
                else
                {
                    db.Decorate.Add(decorate);
                }
                db.SaveChanges();
                Layout layout = db.Layout.Find(decorate.LayoutId);
                Building building = db.Building.Find(layout.BuildingId);
                building.UpdateTime = DateTime.Now;
                db.Entry(building).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List", new { id = decorate.LayoutId });
               

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
                Decorate decorate = db.Decorate.Find(id);
                ViewBag.Details = true;
                return View("Edit", decorate);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        public ActionResult Delete(int id,string returnURL)
        {
            if (Session["admin"] != null)
            {
                Decorate decorate = db.Decorate.Find(id);
                db.Decorate.Remove(decorate);
                db.SaveChanges();
                return Redirect(returnURL);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        public string ToSmall(string originalImagePath)
        {
            bool isExist = WXSSK.Common.DirectoryAndFile.FileExists(originalImagePath);
            System.Drawing.Image imgOriginal = System.Drawing.Image.FromFile(Server.MapPath(originalImagePath));

            //获取原图片的的宽度与高度
            int originalWidth = imgOriginal.Width;
            int originalHeight = imgOriginal.Height;

            //originalHeight = (int)originalHeight * intWidth / originalWidth;  //高度做相应比例缩小
            //originalWidth = intWidth;  //宽度等于缩略图片尺寸

            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(originalWidth, originalHeight);
            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);

            //设置缩略图片质量
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            graphics.DrawImage(imgOriginal, 0, 0, originalWidth, originalHeight);

            // 保存缩略图片
            imgOriginal.Dispose();
            WXSSK.Common.DirectoryAndFile.DeleteFile(originalImagePath);
            bitmap.Save(Server.MapPath(originalImagePath), System.Drawing.Imaging.ImageFormat.Jpeg);
            return originalImagePath;
        }
	}
}