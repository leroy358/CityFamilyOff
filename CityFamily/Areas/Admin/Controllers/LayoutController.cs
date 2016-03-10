using CityFamily.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityFamily.Areas.Admin.Controllers
{
    public class LayoutController : Controller
    {
        private CityFamilyDbContext db = new CityFamilyDbContext();
        int pageSize = 10;
        public ActionResult List(int id, string searchStr, int pageIndex = 1)
        {
            if (Session["admin"] != null)
            {
                ViewBag.searchStr = searchStr;
                var layouts = db.Layout.Where(item => item.BuildingId == id);
                ViewBag.BuildId = id;
                Building build = db.Building.Find(id);
                ViewBag.BuildName = build.BuildingName;
                if (!string.IsNullOrEmpty(searchStr))
                {
                    int iSearch = 0;
                    int.TryParse(searchStr, out iSearch);
                    if (iSearch == 0)
                    {
                        layouts = layouts.Where(item => item.LayoutName.Contains(searchStr)).OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                        int count = layouts.Count();
                        InitPage(pageIndex, count, searchStr, id);
                        return View(layouts);
                    }
                    else
                    {
                        layouts = layouts.Where(item => item.Id == iSearch||item.LayoutName.Contains(searchStr));
                        ViewBag.pageX = 1;
                        ViewBag.pageCount = 1;
                        return View(layouts);
                    }
                }
                else
                {
                    layouts = layouts.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                    int count = db.Layout.Where(item => item.BuildingId == id).Count();
                    InitPage(pageIndex, count, searchStr, id);
                    return View(layouts);
                }
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
                Building build = db.Building.Find(id);
                ViewBag.BuildName = build.BuildingName;
                Layout layout = new Layout();
                layout.BuildingId = id;
                return View("Edit", layout);
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
                Layout layout = db.Layout.Find(id);
                Building build = db.Building.Find(layout.BuildingId);
                ViewBag.BuildName = build.BuildingName;
                ViewBag.IsCreate = true;
                return View(layout);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        [HttpPost]
        public ActionResult SavEdit(Layout layout, bool IsCreate)
        {
            if (Session["admin"] != null)
            {
                layout.CreateTime = DateTime.Now.ToString("yyyy-MM-dd");
                layout.LayoutPic = ToSmall(layout.LayoutPic);
                if (IsCreate)
                {
                    db.Entry(layout).State = EntityState.Modified;
                }
                else
                {
                    db.Layout.Add(layout);
                }
                db.SaveChanges();

                string buildingId = layout.BuildingId.ToString();
                UpdateRecord record = db.UpdateRecord.Where(item => item.BuildingId == buildingId).FirstOrDefault();
                if (record != null)
                {
                    record.UpdateTime = DateTime.Now;
                    db.Entry(record).State = EntityState.Modified;
                }
                else
                {
                    record = new UpdateRecord();
                    record.BuildingId = buildingId;
                    record.UpdateTime = DateTime.Now;
                    db.UpdateRecord.Add(record);
                }
                db.SaveChanges();
                return RedirectToAction("List", new { id = layout.BuildingId });
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
                Layout layout = db.Layout.Find(id);
                db.Layout.Remove(layout);
                List<Decorate> decorates = db.Decorate.Where(item => item.LayoutId == id).ToList();
                foreach (Decorate decorate in decorates)
                {
                    db.Decorate.Remove(decorate);
                }
                string buildingId = layout.BuildingId.ToString();
                UpdateRecord record = db.UpdateRecord.Where(item => item.BuildingId == buildingId).FirstOrDefault();
                if (record != null)
                {
                    record.UpdateTime = DateTime.Now;
                    db.Entry(record).State = EntityState.Modified;
                }
                else
                {
                    record = new UpdateRecord();
                    record.BuildingId = buildingId;
                    record.UpdateTime = DateTime.Now;
                    db.UpdateRecord.Add(record);
                }
                db.SaveChanges();
                return Redirect(returnURL);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        private void InitPage(int pageIndex, int count, string searchStr,int id)
        {
            if (count == 0)
            {
                count = 1;
            }
            int pageCount = (count % pageSize == 0) ? count / pageSize : count / pageSize + 1;
            string perPage = Url.Action("List", new { id, searchStr, pageIndex = (pageIndex < 2) ? 1 : pageIndex - 1});
            string nextPage = Url.Action("List", new { id, searchStr, pageIndex = (pageIndex == pageCount) ? pageCount : (pageIndex + 1) });
            string lastPage = Url.Action("List", new { id, searchStr, pageIndex = pageCount });
            string firstPage = Url.Action("List", new { id, searchStr, pageIndex = 1 });
            string pageX = Url.Action("List", new { id, searchStr, pageIndex });
            ViewBag.perPage = perPage;
            ViewBag.nextPage = nextPage;
            ViewBag.pageCount = pageCount;
            ViewBag.pageX = pageX.Substring(pageX.Length - 1, 1);
            ViewBag.lastPage = lastPage;
            ViewBag.firstPage = firstPage;
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