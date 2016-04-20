using CityFamily.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityFamily.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private CityFamilyDbContext db = new CityFamilyDbContext();
        int pageSize = 10;
        public ActionResult List(string searchStr,int pageIndex = 1)
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
                        var users = db.Admins.Where(item => item.Id == iSearch||item.AdminName.Contains(searchStr));
                        return View(users);
                    }
                    else
                    {
                        var users = db.Admins.Where(item => item.AdminName.Contains(searchStr));
                        int count = users.Count();
                        users = users.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                        InitPage(pageIndex, count, searchStr);
                        return View(users);
                    }
                }
                else
                {
                    var users = db.Admins.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                    int count = db.Admins.Count();
                    InitPage(pageIndex, count, searchStr);
                    return View(users);
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
                return View("Edit", new Admins());
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
                var user = db.Admins.Find(id);
                ViewBag.IsCreate = true;
                return View(user);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }
        [HttpPost]
        public ActionResult SavEdit(string ReturnURL, string UserName,string Password, bool IsCreate,string newPassword,string confirmPassword)
        {
            if (Session["admin"] != null)
            {
                if (IsCreate)
                {
                    if (!string.IsNullOrEmpty(newPassword.Trim()) && !string.IsNullOrEmpty(confirmPassword.Trim()))
                    {
                        if (newPassword == confirmPassword)
                        {
                            if (!string.IsNullOrEmpty(Password))
                            {
                                Admins user = db.Admins.Where(item => item.AdminName == UserName && item.Password == Password).FirstOrDefault();
                                if (user != null)
                                {
                                    user.Password = confirmPassword;
                                    db.Entry(user).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    return Content("<script>alert('初始密码输入不正确，请重新输入！');window.location.href='" + ReturnURL + "'</script>");
                                }
                            }
                            else
                            {
                                return Content("<script>alert('初始密码不能为空，请重新输入！');window.location.href='" + ReturnURL + "'</script>");
                            }
                        }
                        else
                        {
                            return Content("<script>alert('两次密码输入不一致，请重新输入！');window.location.href='" + ReturnURL + "'</script>");
                        }
                    }
                    else
                    {
                        return Content("<script>alert('密码不能为空，请重新输入！');window.location.href='"+ ReturnURL +"'</script>");
                    }
                    return Content("<script>alert('修改密码成功！');window.location.href='List'</script>");
                }
                else
                {
                    if (!string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(UserName))
                    {
                        Admins user = new Admins();
                        user.AdminName = UserName;
                        user.Password = Password;
                        db.Admins.Add(user);
                        db.SaveChanges();
                    }
                    else
                    {
                        return Content("<script>alert('用户名或密码不能为空，请重新输入！');window.location.href='" + ReturnURL + "'</script>");
                    }
                    return Content("<script>alert('添加用户成功！');window.location.href='List'</script>");
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
                var user = db.Admins.Find(id);
                db.Admins.Remove(user);
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
            ViewBag.pageX = pageIndex;
            ViewBag.lastPage = lastPage;
            ViewBag.firstPage = firstPage;
        }
        //public ActionResult CheckUser(string username)
        //{
        //    var users = db.Admins.Where(item => item.AdminName == username).FirstOrDefault();
        //    if (users != null)
        //    {
        //        return Json("Error");
        //    }
        //    else
        //    {
        //        return Json("Success");
        //    }
        //}
	}
}