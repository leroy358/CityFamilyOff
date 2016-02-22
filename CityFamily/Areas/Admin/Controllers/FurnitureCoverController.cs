using CityFamily.Areas.Admin.Models;
using CityFamily.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityFamily.Areas.Admin.Controllers
{
    public class FurnitureCoverController : Controller
    {
        //
        // GET: /Admin/FurnitureCover/
        public ActionResult Index()
        {
            return View();
        }


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
                    var style = db.T_FurnitureCover.Where(item => (adminId == 1 ? 1 == 1 : item.CompanyId == companyId) && item.StyleName.Contains(searchStr)).OrderByDescending(item => item.Id);
                    return View(style);
                }
                else
                {
                    var styles = db.T_FurnitureCover.Where(item => (adminId == 1 ? 1 == 1 : item.CompanyId == companyId)).OrderByDescending(item => item.Id);
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
                //var objModel = db.T_CompanyInfo.Where(o => o.CompanyID == getSession.CompanyId).FirstOrDefault();
                var fstyleidmodel = db.FCoverID.Where(o => o.CompanyId == getSession.CompanyId).Select(o => o.FCoverId).ToList();
                var objModel = db.T_FurnitureCover.Where(o => o.CompanyId == 0 || o.CompanyId == 6).ToList();
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
                return View("Edit", new T_FurnitureCover());
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
                T_FurnitureCover style = db.T_FurnitureCover.Find(id);
                ViewBag.IsCreate = true;
                return View(style);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }


        [HttpPost]
        public ActionResult SavEdit(bool IsCreate, T_FurnitureCover style)
        {
            if (Session["admin"] != null)
            {
                style.CreateTime = DateTime.Now;
                if (IsCreate)
                {
                    db.Entry(style).State = EntityState.Modified;
                    style.CompanyId = getSession.CompanyId;
                    style.CreateUserId = getSession.AdminId;
                    int ll = db.SaveChanges();
                }
                else
                {
                    style.CompanyId = getSession.CompanyId;
                    style.CreateUserId = getSession.AdminId;
                    style.FStyleState = 1; //默认显示
                    db.T_FurnitureCover.Add(style);
                    int ll = db.SaveChanges();
                }
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
                T_FurnitureCover style = db.T_FurnitureCover.Find(id);
                db.T_FurnitureCover.Remove(style);
                db.SaveChanges();


                List<FurnitureStyle> sftyle = db.FurnitureStyle.Where(o => o.CompanyId == getSession.CompanyId && o.StyleId == id).ToList();
                db.FurnitureStyle.RemoveRange(sftyle);
                db.SaveChanges();

                return Redirect(returnURL);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }

        [HttpPost]
        public ActionResult SavShield(int IsShield=0)
        {
            if (Session["admin"] != null)
            {
                var objModel = db.T_CompanyInfo.Where(o => o.CompanyID == getSession.CompanyId).FirstOrDefault();
                objModel.IsFurnitureCoverShield = IsShield;
                db.Entry(objModel).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect("List");
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }




        public ActionResult ShowData(int coverid, int state)
        {
            if (Session["admin"] != null)
            {
                if (state == 0)
                {
                    FCoverID fstyleid = new FCoverID();
                    fstyleid.CompanyId = getSession.CompanyId;
                    fstyleid.FCoverId = coverid;
                    fstyleid.CreateTime = DateTime.Now;
                    fstyleid.CreateUserId = getSession.AdminId;
                    db.FCoverID.Add(fstyleid);
                    db.SaveChanges();
                }
                else
                {
                    FCoverID fstyleid = db.FCoverID.Where(o => o.FCoverId == coverid && o.CompanyId == getSession.CompanyId).FirstOrDefault();
                    db.FCoverID.Remove(fstyleid);
                    db.SaveChanges();
                }
                return RedirectToAction("Shield", "FurnitureCover");
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }


    }
}