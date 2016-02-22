using CityFamily.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityFamily.Areas.Admin.Controllers
{
    public class ConfigController : Controller
    {
        private CityFamilyDbContext db = new CityFamilyDbContext();
        int pageSize = 10;

        public ActionResult List(string searchStr, int pageIndex = 1)
        {
            if (Session["admin"] != null)
            {
                var userrole = db.T_UserRole.Where(o => o.RoleID == 2).Select(o => o.UserID).ToList();
                ViewBag.searchStr = searchStr;
                if (!string.IsNullOrEmpty(searchStr))
                {
                    int iSearch = 0;
                    int.TryParse(searchStr, out iSearch);
                    if (iSearch != 0)
                    {
                        ViewBag.pageX = 1;
                        ViewBag.pageCount = 1;

                        var users = db.Admins.Where(item => userrole.Contains(item.Id) && item.LoginState == 1 && item.Id == iSearch || item.AdminName.Contains(searchStr));
                        return View(users);
                    }
                    else
                    {
                        var users = db.Admins.Where(item => userrole.Contains(item.Id) && item.LoginState == 1 && item.AdminName.Contains(searchStr));
                        int count = users.Count();
                        users = users.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                        InitPage(pageIndex, count, searchStr);
                        return View(users);
                    }
                }
                else
                {
                    var users = db.Admins.Where(o => userrole.Contains(o.Id) && o.LoginState == 1).OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                    int count = users.Count();
                    InitPage(pageIndex, count, searchStr);
                    return View(users);
                }
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }

        }

        public ActionResult AddUserInfo(int id)
        {
            if (Session["admin"] != null)
            {
                AdminsCompany ac = new AdminsCompany();
                ac.T_CompanyInfoList = db.T_CompanyInfo.ToList();
                if (id == 0)
                {
                    ac.Admins = new Admins();
                    ViewBag.IsCreate = false;
                    return View(ac);
                }
                else
                {
                    var user = db.Admins.Find(id);
                    ViewBag.IsCreate = true;
                    ac.Admins = user;
                    return View(ac);
                }
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }

        public ActionResult Edit()
        {
            if (Session["admin"] != null)
            {
                int adminid = int.Parse(Session["adminId"].ToString());
                Admins admin = db.Admins.Where(o => o.Id == adminid).FirstOrDefault();
                return View(admin);
            }
            else
            {
                return RedirectToAction("Login", "Console");
            }
        }



        [HttpPost]
        public ActionResult SavEdit(string ReturnURL, string UserName, string Password, bool IsCreate, string newPassword, string confirmPassword, int CompanyName = 0)
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
                        return Content("<script>alert('密码不能为空，请重新输入！');window.location.href='" + ReturnURL + "'</script>");
                    }
                    return Content("<script>alert('修改密码成功！');window.location.href='List'</script>");
                }
                else
                {
                    if (!string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(UserName))
                    {
                        var admins = db.Admins.Where(o => o.CompanyID == CompanyName).FirstOrDefault();
                        if (admins == null)
                        {
                            string sqlstr = "insert Admins(AdminName,Password,CompanyID,LoginState) values('" + UserName + "','" + Password + "'," + CompanyName + ",1)";
                            db.Database.ExecuteSqlCommand(sqlstr);

                            int userid = db.Admins.Where(o => o.AdminName == UserName).FirstOrDefault().Id;
                            int adminid = int.Parse(Session["adminId"].ToString());
                            string sqlstr1 = "insert T_UserRole(RoleID,UserID,CreateUserID,CreateDate) values(2," + userid + "," + adminid + ",getdate())";
                            db.Database.ExecuteSqlCommand(sqlstr1);
                        }
                        else
                        {
                            return Content("<script>alert('该分公司下已经有管理员，账号：" + admins.AdminName + "！');window.location.href='List'</script>");
                        }
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
            ViewBag.pageX = pageX.Substring(pageX.Length - 1, 1);
            ViewBag.lastPage = lastPage;
            ViewBag.firstPage = firstPage;
        }



    }
}