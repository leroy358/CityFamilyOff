using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class T_CompanyInfo
    {
        [Key]
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public int CreateUseID { get; set; }
        public DateTime CreateDate { get; set; }
        public int ModifyUserID { get; set; }
        public DateTime ModifyDate { get; set; }

        public int IsFurnitureStyleShield { get; set; }
        public int IsFurnitureCoverShield { get; set; }
        public int IsStylesShield { get; set; }
        public int IsBuildingShield { get; set; }
    }
}