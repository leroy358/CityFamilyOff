using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class T_MenuInfo
    {

        [Key]
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public int ParentMenuID { get; set; }
        public string MenuURL { get; set; }
        public int Display { get; set; }
        public int Cotyledons { get; set; }


    }
}