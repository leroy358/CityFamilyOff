using CityFamily.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityFamily.Areas.Admin.Controllers
{
    public class DepartmentController : Controller
    {
        //
        // GET: /Admin/Department/
        public ActionResult Index()
        {
            return View();
        }


        private CityFamilyDbContext db = new CityFamilyDbContext();
        int pageSize = 10;

        public ActionResult List(string searchStr, int pageIndex = 1)
        {
            if (Session["admin"] != null)
            {
                int userid = int.Parse(Session["adminId"].ToString());
                ViewBag.searchStr = searchStr;
                if (!string.IsNullOrEmpty(searchStr))
                {
                    int iSearch = 0;
                    int.TryParse(searchStr, out iSearch);
                    if (iSearch != 0)
                    {
                        ViewBag.pageX = 1;
                        ViewBag.pageCount = 1;


                        var department = db.T_DepartmentInfo.Where(item => item.CreateUserID == userid && (item.DepartmentID == iSearch || item.DepartmentName.Contains(searchStr)));
                        return View(department);
                    }
                    else
                    {
                        var department = db.T_DepartmentInfo.Where(item => item.CreateUserID == userid && item.DepartmentName.Contains(searchStr));
                        int count = department.Count();
                        department = department.OrderByDescending(item => item.DepartmentID).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                        InitPage(pageIndex, count, searchStr);
                        return View(department);
                    }
                }
                else
                {
                    var department = db.T_DepartmentInfo.Where(o => o.CreateUserID == userid).OrderByDescending(item => item.DepartmentID).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                    int count = db.T_DepartmentInfo.Count();
                    InitPage(pageIndex, count, searchStr);
                    return View(department);
                }
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
                if (id == 0)
                {
                    ViewBag.IsCreate = false;
                    return View(new T_DepartmentInfo());
                }
                else
                {
                    var department = db.T_DepartmentInfo.Find(id);
                    ViewBag.IsCreate = true;
                    return View(department);
                }
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



        [HttpPost]
        public ActionResult SavEdit(string ReturnURL, string DepartmentName, bool IsCreate, int DepartmentID)
        {
            if (Session["admin"] != null)
            {
                if (IsCreate)
                {
                    if (!string.IsNullOrEmpty(DepartmentName.Trim()))
                    {
                        T_DepartmentInfo department = db.T_DepartmentInfo.Where(item => item.DepartmentID == DepartmentID).FirstOrDefault();
                        if (department != null)
                        {
                            department.DepartmentName = DepartmentName;
                            db.Entry(department).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            return Content("<script>alert('部门ID输入不正确，请重新输入！');window.location.href='" + ReturnURL + "'</script>");
                        }
                    }
                    else
                    {
                        return Content("<script>alert('部门名称不能为空，请重新输入！');window.location.href='" + ReturnURL + "'</script>");
                    }
                    return Content("<script>alert('修改部门名称成功！');window.location.href='List'</script>");
                }
                else
                {
                    if (!string.IsNullOrEmpty(DepartmentName))
                    {
                        try
                        {

                            T_DepartmentInfo department = new T_DepartmentInfo();
                            department.DepartmentName = DepartmentName;
                            department.DepartmentLevel = 1;
                            department.DepartmentDesc = "";
                            department.CreateUserID = int.Parse(Session["adminId"].ToString());
                            department.CreateDate = DateTime.Now;
                            department.CompanyID = int.Parse(Session["cid"].ToString());
                            department.ModifyDate = DateTime.Now;
                            department.ModifyUserID = int.Parse(Session["adminId"].ToString());
                            db.T_DepartmentInfo.Add(department);

                            db.SaveChanges();

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    else
                    {
                        return Content("<script>alert('部门名称不能为空，请重新输入！');window.location.href='" + ReturnURL + "'</script>");
                    }
                    return Content("<script>alert('添加部门成功！');window.location.href='List'</script>");
                }

            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }



    }
}