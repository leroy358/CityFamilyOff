using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityFamily.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }
        [OutputCache(CacheProfile = "SqlDependencyCache")]
        public ActionResult Test()
        {
            System.Web.Caching.SqlCacheDependencyAdmin.EnableNotifications(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);//通知哪个数据库,name是该数据库链接字符串的名字
            System.Web.Caching.SqlCacheDependencyAdmin.EnableTableForNotifications(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, "Users");//通知启用哪个表，Player是表名
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return Content(time);
        }
    }
}