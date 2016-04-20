using CityFamily.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CityFamily.Areas.Admin.Controllers
{
    public class StyleDetailController : Controller
    {
        private CityFamilyDbContext db = new CityFamilyDbContext();
        int pageSize = 10;
        public ActionResult List(int id, string searchStr)
        {
            if (Session["admin"] != null)
            {
                Styles style = db.Styles.Find(id);
                ViewBag.StyleName = style.StyleName;
                ViewBag.StyleId = id;
                ViewBag.searchStr = searchStr;
                if (!string.IsNullOrEmpty(searchStr))
                {
                    var styleDetail = db.StyleDetails.Where(item => item.StyleId == id).OrderByDescending(item => item.Id).Where(item => item.StyleDetailName.Contains(searchStr));
                    return View(styleDetail);
                }
                else
                {
                    var styleDetail = db.StyleDetails.Where(item => item.StyleId == id).OrderByDescending(item => item.Id);
                    return View(styleDetail);
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
                Styles style = db.Styles.Find(id);
                ViewBag.StyleName = style.StyleName;
                ViewBag.StyleId = id;
                ViewBag.IsCreate = false;
                StyleDetails styleDetail = new StyleDetails();
                styleDetail.StyleId = id;
                return View("Edit", styleDetail);
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
                StyleDetails styleDetail = db.StyleDetails.Find(id);
                Styles style = db.Styles.Find(styleDetail.StyleId);
                ViewBag.StyleName = style.StyleName;
                ViewBag.StyleId = style.Id;
                return View(styleDetail);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        [HttpPost]
        public ActionResult SavEdit(bool IsCreate, StyleDetails styleDetail)
        {
            if (Session["admin"] != null)
            {
                styleDetail.CreateTime = DateTime.Now.ToString("yyyy-MM-dd");
                styleDetail.StyleDetailIndex = ToSmall(styleDetail.StyleDetailIndex);
                if (IsCreate)
                {
                    db.Entry(styleDetail).State = EntityState.Modified;
                }
                else
                {
                    db.StyleDetails.Add(styleDetail);
                }
                db.SaveChanges();

                string styleId = styleDetail.StyleId.ToString();
                UpdateRecord record = db.UpdateRecord.Where(item => item.StyleId == styleId).FirstOrDefault();
                if (record != null)
                {
                    record.UpdateTime = DateTime.Now;
                    db.Entry(record).State = EntityState.Modified;
                }
                else
                {
                    record = new UpdateRecord();
                    record.StyleId = styleId;
                    record.UpdateTime = DateTime.Now;
                    db.UpdateRecord.Add(record);
                }
                db.SaveChanges();

                return RedirectToAction("List", new { id = styleDetail.StyleId });
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
                StyleDetails styleDetail = db.StyleDetails.Find(id);
                db.StyleDetails.Remove(styleDetail);

                string styleId = styleDetail.StyleId.ToString();
                UpdateRecord record = db.UpdateRecord.Where(item => item.StyleId == styleId).FirstOrDefault();
                if (record != null)
                {
                    record.UpdateTime = DateTime.Now;
                    db.Entry(record).State = EntityState.Modified;
                }
                else
                {
                    record = new UpdateRecord();
                    record.StyleId = styleId;
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

        public ActionResult DIYList(string searchStr, int pageIndex = 1)
        {
            if (Session["admin"] != null)
            {
                int adminid = int.Parse(Session["adminId"].ToString());
                List<int> td = null;
                if (adminid == 1)
                {
                    td = db.DIYResult.Select(o => o.UserId).ToList();
                }
                else
                {
                    List<int> up = db.T_UserDepartment.Where(o => o.UserID == adminid).Select(o => o.DepartmentID).ToList();
                    td = db.T_UserDepartment.Where(o => up.Contains(o.DepartmentID)).Select(o => o.UserID).ToList();
                }
                if (string.IsNullOrEmpty(searchStr))
                {
                    var diyResult = db.DIYResult.Where(o => td.Contains(o.UserId)).OrderByDescending(item => item.Id);
                    int count = diyResult.Count();
                    InitPage(pageIndex, count, searchStr);
                    return View(diyResult);
                }
                else
                {
                    ViewBag.searchStr = searchStr;
                    var diyResult = db.DIYResult.Where(item => td.Contains(item.UserId) && item.GuestName.Contains(searchStr) || item.UserName.Contains(searchStr)).OrderByDescending(item => item.Id);
                    int count = diyResult.Count();
                    InitPage(pageIndex, count, searchStr);
                    return View(diyResult);
                }
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }

        public ActionResult DIYDetail(int id)
        {
            if (Session["admin"] != null)
            {
                DIYResult result = db.DIYResult.Find(id);
                StyleThird styleThird = db.StyleThird.Find(result.StyleDetailId);
                ViewBag.styleCode = styleThird.StyleThirdCode;
                ViewBag.styleResource = styleThird.StyleResource;
                return View(result);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        private void InitPage(int pageIndex, int count, string searchStr)
        {
            int pageCount = (count % pageSize == 0) ? count / pageSize : count / pageSize + 1;
            if (pageCount == 0)
            {
                pageCount = 1;
            }
            string perPage = Url.Action("DIYList", new { searchStr, pageIndex = (pageIndex < 2) ? 1 : pageIndex - 1 });
            string nextPage = Url.Action("DIYList", new { searchStr, pageIndex = (pageIndex == pageCount) ? pageCount : (pageIndex + 1) });
            string lastPage = Url.Action("DIYList", new { searchStr, pageIndex = pageCount });
            string firstPage = Url.Action("DIYList", new { searchStr, pageIndex = 1 });
            string pageX = Url.Action("DIYList", new { searchStr, pageIndex });
            ViewBag.perPage = perPage;
            ViewBag.nextPage = nextPage;
            ViewBag.pageCount = pageCount;
            ViewBag.pageX = pageX.Substring(pageX.Length - 1, 1);
            ViewBag.lastPage = lastPage;
            ViewBag.firstPage = firstPage;
        }

        public static JsonData ScriptDeserialize(string strJson)
        {

            JavaScriptSerializer js = new JavaScriptSerializer();

            return js.Deserialize<JsonData>(strJson);
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