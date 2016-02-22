﻿using CityFamily.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityFamily.Controllers
{
    public class FurnitureViewController : Controller
    {
        private CityFamilyDbContext db = new CityFamilyDbContext();
        GetWebSession websession = new GetWebSession();
        int pageSize = 12;
        public ActionResult List(int styleid, int pageIndex = 1)
        {
            if (Session["userName"] != null)
            {
                int adminId = websession.AdminId;
                int companyId = websession.CompanyId;
                //var furniture = db.FurnitureStyle.Where(o => o.StyleId == styleid).OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                //var furniture = db.FurnitureStyle.Where(o => o.StyleId == styleid).Select(o => o.IdentityId).ToList();
                //var pics = db.T_StyleFurniturePics.Where(o => furniture.Contains(o.IdentityId));
                //var furnitureStylePics = pics.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                //int count = pics.Count();
                //InitPage(pageIndex, count, styleid);
                //FurnitureStylePics fsmodel = new FurnitureStylePics();
                //fsmodel.Id = 1;
                //fsmodel.StyleName = (styleid == 1 ? "欧式风格" : (styleid == 2 ? "中式风格" : "现代风格"));
                //fsmodel.T_StyleFurniturePicsList = furnitureStylePics;

                var companyModel = db.T_CompanyInfo.Where(o => o.CompanyID == companyId).FirstOrDefault();
                //var fsmodelAll = db.FurnitureStyle.Where(o => (companyModel.IsFurnitureCoverShield == 1 ? o.CompanyId == companyId : (o.CreateUserId == 1 || o.CompanyId == companyId)) && o.StyleId == styleid).ToList();
                //var fsmodel = db.FurnitureStyle.Where(o => (companyModel.IsFurnitureCoverShield == 1 ? o.CompanyId == companyId : (o.CreateUserId == 1 || o.CompanyId == companyId)) && o.StyleId == styleid).OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();

                var notshowdata = db.FStyleID.Where(o => o.CompanyId == companyId).Select(o => o.FStyleId).ToList();
                var fsmodelAll = db.FurnitureStyle.Where(item => item.StyleId == styleid).Where(o => o.CompanyId == companyId || (o.CreateUserId == 1 && !notshowdata.Contains(o.Id))).ToList();
                var fsmodel = fsmodelAll.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();

                int count = fsmodelAll.Count();
                InitPage(pageIndex, count, styleid);
                return View(fsmodel);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        private void InitPage(int pageIndex, int count, int styleid)
        {
            int pageCount = (count % pageSize == 0) ? count / pageSize : count / pageSize + 1;
            if (pageCount == 0)
            {
                pageCount = 1;
            }
            string perPage = Url.Action("List", new { styleid, pageIndex = (pageIndex < 2) ? 1 : pageIndex - 1 });
            string nextPage = Url.Action("List", new { styleid, pageIndex = (pageIndex == pageCount) ? pageCount : (pageIndex + 1) });
            string lastPage = Url.Action("List", new { styleid, pageIndex = pageCount });
            string firstPage = Url.Action("List", new { styleid, pageIndex = 1 });
            string pageX = Url.Action("List", new { styleid, pageIndex });
            ViewBag.perPage = perPage;
            ViewBag.nextPage = nextPage;
            ViewBag.pageCount = pageCount;
            ViewBag.pageX = pageX.Substring(pageX.Length - 1, 1);
            ViewBag.lastPage = lastPage;
            ViewBag.firstPage = firstPage;
        }


        public ActionResult CoverList()
        {
            if (Session["userName"] != null)
            {
                int adminId = websession.AdminId;
                int companyId = websession.CompanyId;
                //var companyModel = db.T_CompanyInfo.Where(o => o.CompanyID == companyId).FirstOrDefault();
                var notshowdata = db.FCoverID.Where(o => o.CompanyId == companyId).Select(o => o.FCoverId).ToList();
                List<T_FurnitureCover> list = db.T_FurnitureCover.Where(o => o.CompanyId == companyId || (o.CreateUserId == 1 && !notshowdata.Contains(o.Id))).ToList();
                return View(list);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult Detail(int id)
        {
            if (Session["userName"] != null)
            {
                var furnitureStylePics = db.FurnitureStyle.Find(id);
                //int styleid = furnitureStylePics.StyleId;
                //FurnitureStylePics fsmodel = new FurnitureStylePics();
                //fsmodel.StyleId = styleid;
                //fsmodel.StyleName = (styleid == 1 ? "欧式风格" : (styleid == 2 ? "中式风格" : "现代风格"));
                //fsmodel.BackPic = db.FurnitureStyle.Where(o => o.StyleId == styleid).FirstOrDefault().BackPic;
                //fsmodel.T_StyleFurniturePicsList = db.T_StyleFurniturePics.Where(o => o.StyleId == styleid).ToList();
                //fsmodel.Id = id;
                return View(furnitureStylePics);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        public ActionResult SaveSessionPic(string pic, int Id, int ModelId)
        {
            if (Session["userName"] != null)
            {
                if (Session["FurnitureViewUrl"] != null)
                {
                    string pics = Session["FurnitureViewUrl"].ToString();
                    if (pics.Contains(pic))
                    {
                        Session["FurnitureViewUrl"] = pics;
                    }
                    else
                    {
                        Session["FurnitureViewUrl"] = pics + "&" + pic;
                    }
                }
                else
                {
                    Session["FurnitureViewUrl"] = pic;
                }
                return RedirectToAction("Detail", new { id = ModelId });
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

    }
}