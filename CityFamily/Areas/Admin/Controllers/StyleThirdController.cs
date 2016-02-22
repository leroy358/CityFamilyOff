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
    public class StyleThirdController : Controller
    {
        private CityFamilyDbContext db = new CityFamilyDbContext();
        public ActionResult List(int id)
        {
            if (Session["admin"] != null)
            {
                var styleThirds = db.StyleThird.Where(item => item.StyleDetailId == id).OrderByDescending(item => item.Id);
                ViewBag.StyleDetailId = id;
                StyleDetails styleDetail = db.StyleDetails.Find(id);
                ViewBag.StyleDetailName = styleDetail.StyleDetailName;
                Styles style = db.Styles.Find(styleDetail.StyleId);
                ViewBag.StyleName = style.StyleName;
                ViewBag.StyleId = style.Id;
                return View(styleThirds);
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
                StyleThird styleThird = new StyleThird();
                styleThird.StyleDetailId = id;
                ViewBag.StyleDetailId = id;
                StyleDetails styleDetail = db.StyleDetails.Find(id);
                ViewBag.StyleDetailName = styleDetail.StyleDetailName;
                Styles style = db.Styles.Find(styleDetail.StyleId);
                ViewBag.StyleName = style.StyleName;
                ViewBag.StyleId = style.Id;
                return View("Edit", styleThird);
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
                ViewBag.IsCreate = true;
                StyleThird styleThird = db.StyleThird.Find(id);                
                StyleDetails styleDetail = db.StyleDetails.Find(styleThird.StyleDetailId);
                ViewBag.StyleDetailName = styleDetail.StyleDetailName;
                ViewBag.StyleDetailId = styleDetail.Id;
                Styles style = db.Styles.Find(styleDetail.StyleId);
                ViewBag.StyleName = style.StyleName;
                ViewBag.StyleId = style.Id;
                return View(styleThird);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        [HttpPost]
        public ActionResult SavEdit(StyleThird styleThird, bool IsCreate)
        {
            if (Session["admin"] != null)
            {
                styleThird.CreateTime = DateTime.Now.ToString("yyyy-MM-dd");
                styleThird.StyleThirdIndex = ToSmall(styleThird.StyleThirdIndex);
                StringBuilder sb = new StringBuilder();
                string[] pics = styleThird.StyleThirdPics.Substring(0, styleThird.StyleThirdPics.Length - 1).Split(' ');
                foreach (var pic in pics)
                {
                    sb.Append(ToSmall(pic));
                    sb.Append(" ");
                }
                styleThird.StyleThirdPics = sb.ToString();
                if (IsCreate)
                {
                    db.Entry(styleThird).State = EntityState.Modified;
                }
                else
                {
                    db.StyleThird.Add(styleThird);
                }
                db.SaveChanges();
                return RedirectToAction("List", new { id = styleThird.StyleDetailId });
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        public ActionResult Delete(int id, string returnURL)
        {
            if (Session["admin"] != null)
            {
                StyleThird styleThird=db.StyleThird.Find(id);
                db.StyleThird.Remove(styleThird);
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