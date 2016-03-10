using CityFamily.Areas.Admin.Models;
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
    public class BuildingController : Controller
    {
        private CityFamilyDbContext db = new CityFamilyDbContext();
        GetSession getSession = new GetSession();
        int pageSize = 10;
        public ActionResult List(string searchStr, int pageIndex = 1)
        {
            if (Session["admin"] != null)
            {
                ViewBag.searchStr = searchStr;

                int adminId = getSession.AdminId;
                int companyId = getSession.CompanyId;

                if (!string.IsNullOrEmpty(searchStr))
                {
                    int iSearch = 0;
                    int.TryParse(searchStr, out iSearch);
                    if (iSearch == 0)
                    {
                        var buildings = db.Building.Where(item => item.BuildingName.Contains(searchStr) && (adminId == 1 ? 1 == 1 : item.CompanyId == companyId));
                        int count = buildings.Count();
                        InitPage(pageIndex, count, searchStr);
                        buildings = buildings.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                        return View(buildings);
                    }
                    else
                    {
                        ViewBag.pageX = 1;
                        ViewBag.pageCount = 1;
                        var buildings = db.Building.Where(item => (adminId == 1 ? 1 == 1 : item.CompanyId == companyId) && (item.Id == iSearch || item.BuildingName.Contains(searchStr)));
                        return View(buildings);
                    }
                }
                else
                {
                    var buildings = db.Building.Where(item => (adminId == 1 ? 1 == 1 : item.CompanyId == companyId));
                    int count = buildings.Count();
                    InitPage(pageIndex, count, searchStr);
                    buildings=buildings.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                    return View(buildings);
                }
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        public ActionResult Create()
        {
            if (Session["admin"] != null)
            {
                ViewBag.IsCreate = false;
                return View("Edit", new Building());
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
                var building = db.Building.Find(id);
                ViewBag.IsCreate = true;
                return View(building);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        [HttpPost]
        public ActionResult SavEdit(Building building, bool IsCreate)
        {
            if (Session["admin"] != null)
            {
                building.CreateTime = DateTime.Now.ToString("yyyy-MM-dd");
                building.BuildingIndex = ToSmall(building.BuildingIndex);
                string[] pics = building.BuildingPics.Substring(0, building.BuildingPics.Length - 1).Split(' ');
                StringBuilder sb = new StringBuilder();
                foreach (var pic in pics)
                {
                    sb.Append(ToSmall(pic));
                    sb.Append(" ");
                }
                building.BuildingPics = sb.ToString();
                if (IsCreate)
                {
                    /////////////////////////////////////////////////////////////////////////////////
                    building.CompanyId = getSession.CompanyId;
                    building.CreateUserId = getSession.AdminId;
                    /////////////////////////////////////////////////////////////////////////////////
                    db.Entry(building).State = EntityState.Modified;
                }
                else
                {
                    building.CompanyId = getSession.CompanyId;
                    building.CreateUserId = getSession.AdminId;
                    db.Building.Add(building);
                }
                db.SaveChanges();
                string buildingId = building.Id.ToString();
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
                return RedirectToAction("List");
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
                Building building = db.Building.Find(id);
                db.Building.Remove(building);
                List<Layout> layouts = db.Layout.Where(item => item.BuildingId == id).ToList();
                foreach (Layout layout in layouts)
                {
                    db.Layout.Remove(layout);
                    List<Decorate> decorates = db.Decorate.Where(item => item.LayoutId == layout.Id).ToList();
                    foreach (Decorate decorate in decorates)
                    {
                        db.Decorate.Remove(decorate);
                    }
                }
                string buildingId = building.Id.ToString();
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
        private void InitPage(int pageIndex, int count, string searchStr)
        {
            if (count == 0)
            {
                count = 1;
            }
            int pageCount = (count % pageSize == 0) ? count / pageSize : count / pageSize + 1;
            string perPage = Url.Action("List", new { searchStr, pageIndex = (pageIndex < 2) ? 1 : pageIndex - 1 });
            string nextPage = Url.Action("List", new { searchStr, pageIndex = (pageIndex == pageCount) ? pageCount : (pageIndex + 1) });
            string lastPage = Url.Action("List", new { searchStr, pageIndex = pageCount });
            string firstPage = Url.Action("List", new { searchStr, pageIndex = 1 });
            string pageX = Url.Action("List", new { searchStr, pageIndex });
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