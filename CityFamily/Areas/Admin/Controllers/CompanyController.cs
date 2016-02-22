using CityFamily.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityFamily.Areas.Admin.Controllers
{
    public class CompanyController : Controller
    {
        //
        // GET: /Admin/Company/
        public ActionResult Index()
        {
            return View();
        }




        private CityFamilyDbContext db = new CityFamilyDbContext();
        int pageSize = 10;

        private int UserId
        {
            get
            {
                if (Session["adminId"] != null)
                {
                    return int.Parse(Session["adminId"].ToString());
                }
                return 0;
            }
        }

        private int CompanyId
        {
            get
            {
                if (Session["uid"] != null)
                {
                    return int.Parse(Session["uid"].ToString());
                }
                return 0;
            }
        }


        public ActionResult List(string searchStr, int pageIndex = 1)
        {
            if (Session["admin"] != null)
            {
                ViewBag.searchStr = searchStr;
                if (!string.IsNullOrEmpty(searchStr))
                {
                    int iSearch = 0;
                    int.TryParse(searchStr, out iSearch);
                    if (iSearch != 0)
                    {
                        ViewBag.pageX = 1;
                        ViewBag.pageCount = 1;
                        var companys = db.T_CompanyInfo.Where(item => item.CompanyID == iSearch || item.CompanyName.Contains(searchStr));
                        return View(companys);
                    }
                    else
                    {
                        var companys = db.T_CompanyInfo.Where(item => item.CompanyName.Contains(searchStr));
                        int count = companys.Count();
                        companys = companys.OrderByDescending(item => item.CompanyID).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                        InitPage(pageIndex, count, searchStr);
                        return View(companys);
                    }
                }
                else
                {
                    var companys = db.T_CompanyInfo.OrderByDescending(item => item.CompanyID).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                    int count = db.T_CompanyInfo.Count();
                    InitPage(pageIndex, count, searchStr);
                    return View(companys);
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
                return View("Edit", new T_CompanyInfo());
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
                var companyinfo = db.T_CompanyInfo.Find(id);
                ViewBag.IsCreate = true;
                return View(companyinfo);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }



        [HttpPost]
        public ActionResult SavEdit(string ReturnURL, string CompanyName, bool IsCreate, int Id = 0)
        {
            if (Session["admin"] != null)
            {
                if (IsCreate)
                {
                    if (!string.IsNullOrEmpty(CompanyName.Trim()))
                    {
                        T_CompanyInfo company = db.T_CompanyInfo.Where(item => item.CompanyID == Id).FirstOrDefault();
                        if (company != null)
                        {
                            company.CompanyName = CompanyName;
                            db.Entry(company).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            return Content("<script>alert('公司ID输入不正确，请重新输入！');window.location.href='" + ReturnURL + "'</script>");
                        }
                    }
                    else
                    {
                        return Content("<script>alert('公司名称不能为空，请重新输入！');window.location.href='" + ReturnURL + "'</script>");
                    }
                    return Content("<script>alert('修改公司名称成功！');window.location.href='List'</script>");
                }
                else
                {
                    if (!string.IsNullOrEmpty(CompanyName))
                    {
                        try
                        {
                            T_CompanyInfo companyinfo = new T_CompanyInfo();
                            companyinfo.CompanyName = CompanyName;
                            companyinfo.CreateUseID = UserId;
                            companyinfo.CreateDate = DateTime.Now;
                            companyinfo.ModifyUserID = UserId;
                            companyinfo.ModifyDate = DateTime.Parse("1900-01-01");
                            db.T_CompanyInfo.Add(companyinfo);
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    else
                    {
                        return Content("<script>alert('公司名称不能为空，请重新输入！');window.location.href='" + ReturnURL + "'</script>");
                    }
                    return Content("<script>alert('添加分公司成功！');window.location.href='List'</script>");
                }

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
                var company = db.T_CompanyInfo.Find(id);
                db.T_CompanyInfo.Remove(company);
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
            int pageCount = (count % pageSize == 0) ? count / pageSize : count / pageSize + 1;
            if (pageCount == 0)
            {
                pageCount = 1;
            }
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






    }
}