using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CityFamily.Models
{
    public class GetWebSession 
    {
        public int AdminId
        {
            get
            {
                if (HttpContext.Current.Session["userId"] != null)
                {
                    return int.Parse(HttpContext.Current.Session["userId"].ToString());
                }
                return 0;
            }
        }


        public int CompanyId
        {
            get
            {
                if (HttpContext.Current.Session["companyId"] != null)
                {
                    return int.Parse(HttpContext.Current.Session["companyId"].ToString());
                }
                return 0;
            }
        }
    }
}