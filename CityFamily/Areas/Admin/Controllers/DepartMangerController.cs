using CityFamily.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityFamily.Areas.Admin.Controllers
{
    public class DepartMangerController : Controller
    {
        //
        // GET: /Admin/DepartManger/
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
                        int cid = int.Parse(Session["cid"].ToString());
                        List<int> deparuser = db.T_UserDepartment.Where(o => o.UserID == userid).Select(o => o.DepartmentID).ToList();
                        List<int> depar = db.T_UserDepartment.Where(o => deparuser.Contains(o.DepartmentID)).Select(o => o.UserID).ToList();
                        var users = db.Admins.Where(item => depar.Contains(item.Id) && (item.Id == iSearch || item.AdminName.Contains(searchStr)));
                        return View(users);
                    }
                    else
                    {
                        int cid = int.Parse(Session["cid"].ToString());
                        List<int> deparuser = db.T_UserDepartment.Where(o => o.UserID == userid).Select(o => o.DepartmentID).ToList();
                        List<int> depar = db.T_UserDepartment.Where(o => deparuser.Contains(o.DepartmentID)).Select(o => o.UserID).ToList();
                        var users = db.Admins.Where(item => depar.Contains(item.Id) && item.AdminName.Contains(searchStr));
                        int count = users.Count();
                        users = users.OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                        InitPage(pageIndex, count, searchStr);
                        return View(users);
                    }
                }
                else
                {
                    int cid = int.Parse(Session["cid"].ToString());
                    List<int> deparuser = db.T_UserDepartment.Where(o => o.UserID == userid).Select(o => o.DepartmentID).ToList();
                    List<int> depar = db.T_UserDepartment.Where(o => deparuser.Contains(o.DepartmentID)).Select(o => o.UserID).ToList();
                    var users = db.Admins.Where(item => depar.Contains(item.Id)).OrderByDescending(item => item.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
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

        public ActionResult Add()
        {
            if (Session["admin"] != null)
            {
                ViewBag.IsCreate = false;
                int cid = int.Parse(Session["cid"].ToString());
                List<T_RoleInfo> role = db.T_RoleInfo.Where(o => o.RoleLevel > 3).ToList();
                DepartmentRole dr = new DepartmentRole();
                dr.DepartmentInfo = null;
                dr.RoleInfo = role;
                return View(dr);
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
        public ActionResult SavEdit(string ReturnURL, string UserName, string Password, bool IsCreate, string newPassword = "", string confirmPassword = "", int Role = 0)
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
                        int cid = int.Parse(Session["cid"].ToString());
                        string sqlstr = "insert Admins(AdminName,Password,LoginState,CompanyID) values('" + UserName + "','" + Password + "',0," + cid + ")";
                        db.Database.ExecuteSqlCommand(sqlstr);

                        int userid = db.Admins.Where(o => o.AdminName == UserName).FirstOrDefault().Id;
                        int adminid = int.Parse(Session["adminId"].ToString());
                        int departid = db.T_UserDepartment.Where(o => o.UserID == adminid).Select(o => o.DepartmentID).FirstOrDefault();

                        string sqlstr1 = "insert T_UserDepartment(UserID,DepartmentID,CreateUserId,CreateDate,ModifyUserID,ModifyDate) values(" + userid + "," + departid + "," + adminid + ",getdate()," + adminid + ",getdate())";

                        db.Database.ExecuteSqlCommand(sqlstr1);

                        string sqlstr2 = "insert T_UserRole(UserID,RoleID,CreateUserId,CreateDate) values(" + userid + "," + Role + "," + adminid + ",getdate())";

                        db.Database.ExecuteSqlCommand(sqlstr2);


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





    }
}