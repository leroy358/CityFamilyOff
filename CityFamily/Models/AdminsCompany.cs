using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class AdminsCompany
    {

        public Admins Admins { get; set; }

        public List<T_CompanyInfo> T_CompanyInfoList { get; set; }

    }
}