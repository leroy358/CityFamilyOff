using CityFamily.Areas.Admin.Models;
using CityFamily.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace CityFamily.Areas.Admin.Controllers
{
    public class StyleController : Controller
    {
        private CityFamilyDbContext db = new CityFamilyDbContext();
        GetSession getSession = new GetSession();
        public ActionResult List(string searchStr)
        {
            if (Session["admin"] != null)
            {
                ViewBag.searchStr = searchStr;

                int adminId = getSession.AdminId;
                int companyId = getSession.CompanyId;

                if (!string.IsNullOrEmpty(searchStr))
                {
                    var style = db.Styles.Where(item => item.StyleName.Contains(searchStr) && (adminId == 1 ? 1 == 1 : item.CompanyId == companyId)).OrderByDescending(item => item.Id);
                    return View(style);
                }
                else
                {
                    var styles = db.Styles.Where(item => (adminId == 1 ? 1 == 1 : item.CompanyId == companyId)).OrderByDescending(item => item.Id);
                    return View(styles);
                }
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }

        public ActionResult Shield()
        {
            if (Session["admin"] != null)
            {
               // var objModel = db.T_CompanyInfo.Where(o => o.CompanyID == getSession.CompanyId).FirstOrDefault();

                var fstyleidmodel = db.StylesID.Where(o => o.CompanyId == getSession.CompanyId).Select(o => o.StylesId).ToList();
                var objModel = db.Styles.Where(o => o.CompanyId == 0 || o.CompanyId == 6).ToList();
                foreach (var item in objModel)
                {
                    int styleid = item.Id;
                    var flag = fstyleidmodel.Contains(styleid);
                    if (flag)
                    {
                        item.FStyleState = 0;
                    }
                }
                return View(objModel);
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
                return View("Edit", new Styles());
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
                Styles style = db.Styles.Find(id);
                ViewBag.IsCreate = true;
                return View(style);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        [HttpPost]
        public ActionResult SavEdit(bool IsCreate, Styles style)
        {
            if (Session["admin"] != null)
            {
                style.CreateTime = DateTime.Now.ToString("yyyy-MM-dd");
                style.StyleIndex = ToSmall(style.StyleIndex);
                if (IsCreate)
                {
                    /////////////////////////////////////////////////////////////////////////////////
                    style.CompanyId = getSession.CompanyId;
                    style.CreateUserId = getSession.AdminId;
                    style.FStyleState = 1;
                    /////////////////////////////////////////////////////////////////////////////////

                    db.Entry(style).State = EntityState.Modified;
                }
                else
                {
                    style.CompanyId = getSession.CompanyId;
                    style.CreateUserId = getSession.AdminId;
                    style.FStyleState = 1; //默认显示
                    db.Styles.Add(style);
                }
                db.SaveChanges();

                string styleId = style.Id.ToString();
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

                return Redirect("List");
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
                Styles style = db.Styles.Find(id);
                db.Styles.Remove(style);

                string styleId = style.Id.ToString();
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


        [HttpPost]
        public ActionResult SavShield(int IsShield = 0)
        {
            if (Session["admin"] != null)
            {
                var objModel = db.T_CompanyInfo.Where(o => o.CompanyID == getSession.CompanyId).FirstOrDefault();
                objModel.IsStylesShield = IsShield;
                db.Entry(objModel).State = EntityState.Modified;
                db.SaveChanges();

                return Redirect("List");
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }

        public ActionResult ShowData(int stylesid, int state)
        {
            if (Session["admin"] != null)
            {
                if (state == 0)
                {
                    StylesID fstyleid = new StylesID();
                    fstyleid.CompanyId = getSession.CompanyId;
                    fstyleid.StylesId = stylesid;
                    fstyleid.CreateTime = DateTime.Now;
                    fstyleid.CreateUserId = getSession.AdminId;
                    db.StylesID.Add(fstyleid);
                    db.SaveChanges();
                }
                else
                {
                    StylesID fstyleid = db.StylesID.Where(o => o.StylesId == stylesid && o.CompanyId == getSession.CompanyId).FirstOrDefault();
                    db.StylesID.Remove(fstyleid);
                    db.SaveChanges();
                }

                string styleId = stylesid.ToString();
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

                return RedirectToAction("Shield", "Style");
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }


    }
}