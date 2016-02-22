using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityFamily.Areas.Admin.Models
{
    public class GetSession
    {

        public int AdminId
        {
            get
            {
                if (HttpContext.Current.Session["adminId"] != null)
                {
                    return int.Parse(HttpContext.Current.Session["adminId"].ToString());
                }
                return 0;
            }
        }


        public int CompanyId
        {
            get
            {
                if (HttpContext.Current.Session["cid"] != null)
                {
                    return int.Parse(HttpContext.Current.Session["cid"].ToString());
                }
                return 0;
            }
        }


    }
}