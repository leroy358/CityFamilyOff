using CityFamily.Areas.Admin.Models;
using CityFamily.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityFamily.Areas.Admin.Controllers
{
    public class FurnitureController : Controller
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
                    var style = db.FurnitureStyle.Where(item => item.StyleName.Contains(searchStr) && (adminId == 1 ? 1 == 1 : item.CompanyId == companyId)).OrderByDescending(item => item.Id);
                    return View(style);
                }
                else
                {
                    var styles = db.FurnitureStyle.Where(item => (adminId == 1 ? 1 == 1 : item.CompanyId == companyId)).OrderByDescending(item => item.Id);
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
                var fstyleidmodel = db.FStyleID.Where(o => o.CompanyId == getSession.CompanyId).Select(o=>o.FStyleId).ToList();
                var objModel = db.FurnitureStyle.Where(o => o.CompanyId == 0 || o.CompanyId == 6).ToList();
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
                return View("Edit", new FurnitureStyle());
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
                FurnitureStyle style = db.FurnitureStyle.Find(id);
                ViewBag.IsCreate = true;
                return View(style);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }


        [HttpPost]
        public ActionResult SavEdit(bool IsCreate, FurnitureStyle style)
        {
            if (Session["admin"] != null)
            {
                style.CreateTime = DateTime.Now.ToString("yyyy-MM-dd");
                //  style.IndexPic = ToSmall(style.IndexPic);

                if (IsCreate)
                {
                    db.Entry(style).State = EntityState.Modified;
                    style.CompanyId = getSession.CompanyId;

                    //style.CompanyId = getSession.CompanyId;
                    //style.CreateUserId = getSession.AdminId;
                    //if (style.IndexPic == "无")
                    //{
                    //    style.IndexPic = ToSmall(style.FurniturePics);
                    //}
                    int ll = db.SaveChanges();
                    // SavePicDataByUpdate(style.FurniturePics, style.StyleId, style.IdentityId);
                }
                else
                {
                    style.IdentityId = Guid.NewGuid().ToString();
                    style.IndexPic = ToSmall(style.FurniturePics);
                    style.CompanyId = getSession.CompanyId;
                    style.CreateUserId = getSession.AdminId;
                    style.FStyleState = 1; //默认显示
                    db.FurnitureStyle.Add(style);
                    int ll = db.SaveChanges();
                    //SavePicData(style.FurniturePics, style.StyleId, style.IdentityId);
                }

                string styleId = style.Id.ToString();
                UpdateRecord record = db.UpdateRecord.Where(item => item.FurnitureId == styleId).FirstOrDefault();
                if (record != null)
                {
                    record.UpdateTime = DateTime.Now;
                    db.Entry(record).State = EntityState.Modified;
                }
                else
                {
                    record = new UpdateRecord();
                    record.FurnitureId = styleId;
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

        [HttpPost]
        public ActionResult SavShield(int IsShield = 0)
        {
            if (Session["admin"] != null)
            {
                var objModel = db.T_CompanyInfo.Where(o => o.CompanyID == getSession.CompanyId).FirstOrDefault();
                objModel.IsFurnitureStyleShield = IsShield;
                db.Entry(objModel).State = EntityState.Modified;
                db.SaveChanges();

                return Redirect("List");
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }

        private void SavePicData(string pics, int styleid, string identityid)
        {
            string[] strs = pics.Split(' ');
            for (int i = 0; i < strs.Length - 1; i++)
            {
                T_StyleFurniturePics sfpics = new T_StyleFurniturePics();
                sfpics.FurniturePics = strs[i];
                sfpics.StyleId = styleid;
                sfpics.IdentityId = identityid;
                sfpics.CreateTime = DateTime.Now;
                db.T_StyleFurniturePics.Add(sfpics);
                db.SaveChanges();
            }
        }

        private void SavePicDataByUpdate(string pics, int styleid, string identityid)
        {
            List<T_StyleFurniturePics> list = db.T_StyleFurniturePics.Where(o => o.IdentityId == identityid).ToList();
            db.T_StyleFurniturePics.RemoveRange(list);
            db.SaveChanges();
            string[] strs = pics.Split(' ');
            for (int i = 0; i < strs.Length - 1; i++)
            {
                T_StyleFurniturePics sfpics = new T_StyleFurniturePics();
                sfpics.FurniturePics = strs[i];
                sfpics.StyleId = styleid;
                sfpics.IdentityId = identityid;
                sfpics.CreateTime = DateTime.Now;
                db.T_StyleFurniturePics.Add(sfpics);
                db.SaveChanges();
            }
        }

        public ActionResult Delete(int id, string returnURL)
        {
            if (Session["admin"] != null)
            {
                FurnitureStyle style = db.FurnitureStyle.Find(id);
                db.FurnitureStyle.Remove(style);
                db.SaveChanges();

                List<T_StyleFurniturePics> styles = db.T_StyleFurniturePics.Where(o => o.IdentityId == style.IdentityId).ToList();
                db.T_StyleFurniturePics.RemoveRange(styles);

                string styleId = style.Id.ToString();
                UpdateRecord record = db.UpdateRecord.Where(item => item.FurnitureId == styleId).FirstOrDefault();
                if (record != null)
                {
                    record.UpdateTime = DateTime.Now;
                    db.Entry(record).State = EntityState.Modified;
                }
                else
                {
                    record = new UpdateRecord();
                    record.FurnitureId = styleId;
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

        public string ToSmall(string originalImagePath)
        {
            bool isExist = WXSSK.Common.DirectoryAndFile.FileExists(originalImagePath);
            System.Drawing.Image imgOriginal = System.Drawing.Image.FromFile(Server.MapPath(originalImagePath));


            //获取原图片的的宽度与高度
            int originalWidth = FurnitureIndexWidth;
            int originalHeight = FurnitureIndexHeight;

            //originalHeight = (int)originalHeight * intWidth / originalWidth;  //高度做相应比例缩小
            //originalWidth = intWidth;  //宽度等于缩略图片尺寸


            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(originalWidth, originalHeight);
            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);


            //设置缩略图片质量
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            graphics.DrawImage(imgOriginal, 0, 0, originalWidth, originalHeight);


            //保存缩略图片
            imgOriginal.Dispose();
            //WXSSK.Common.DirectoryAndFile.DeleteFile(originalImagePath);
            Bitmap bitRevpic = RevPic(bitmap, originalWidth, originalHeight);

            //"/Images/thumbnail/" + originalImagePath.Replace("/Images/data", "");
            string directory = "/Images/thumbnail/" + string.Format("{0:yyyyMM}", DateTime.Now);//this.Request.PhysicalApplicationPath + 
            WXSSK.Common.DirectoryAndFile.CreateDirectory(directory);
            string guid = System.Guid.NewGuid().ToString();
            string thumbnail = directory + "/" + guid + ".jpeg";

            bitRevpic.Save(Server.MapPath(thumbnail), System.Drawing.Imaging.ImageFormat.Jpeg);
            return thumbnail;
        }

        public Bitmap RevPic(Bitmap mybm, int width, int height)
        {
            Bitmap bm = new Bitmap(width, height);//初始化一个记录经过处理后的图片对象
            int x, y, z;//x,y是循环次数,z是用来记录像素点的x坐标的变化的
            Color pixel;

            for (y = height - 1; y >= 0; y--)
            {
                for (x = width - 1, z = 0; x >= 0; x--)
                {
                    pixel = mybm.GetPixel(x, y);//获取当前像素的值
                    bm.SetPixel(z++, y, Color.FromArgb(pixel.R, pixel.G, pixel.B));//绘图
                }
            }

            return bm;//返回经过翻转后的图片
        }

        private int FurnitureIndexWidth
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["FurnitureIndexWidth"]))
                {
                    return int.Parse(ConfigurationManager.AppSettings["FurnitureIndexWidth"]);
                }
                return 250;
            }
        }

        private int FurnitureIndexHeight
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["FurnitureIndexHeight"]))
                {
                    return int.Parse(ConfigurationManager.AppSettings["FurnitureIndexHeight"]);
                }
                return 184;
            }
        }


        public ActionResult ShowData(int styid, int state)
        {
            if (Session["admin"] != null)
            {
                if (state == 0)
                {
                    FStyleID fstyleid = new FStyleID();
                    fstyleid.CompanyId = getSession.CompanyId;
                    fstyleid.FStyleId = styid;
                    db.FStyleID.Add(fstyleid);
                    db.SaveChanges();
                }
                else
                {
                    FStyleID fstyleid = db.FStyleID.Where(o => o.FStyleId == styid && o.CompanyId == getSession.CompanyId).FirstOrDefault();
                    db.FStyleID.Remove(fstyleid);
                    db.SaveChanges();
                }
                FurnitureStyle furniture = db.FurnitureStyle.Find(styid);
                string styleId = furniture.Id.ToString();
                UpdateRecord record = db.UpdateRecord.Where(item => item.FurnitureId == styleId).FirstOrDefault();
                if (record != null)
                {
                    record.UpdateTime = DateTime.Now;
                    db.Entry(record).State = EntityState.Modified;
                }
                else
                {
                    record = new UpdateRecord();
                    record.FurnitureId = styleId;
                    record.UpdateTime = DateTime.Now;
                    db.UpdateRecord.Add(record);
                }
                db.SaveChanges();
                return RedirectToAction("Shield", "Furniture");
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }


    }
}