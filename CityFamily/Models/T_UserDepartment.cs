using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class T_UserDepartment
    {

        [Key]
        public int UserDepartmentID { get; set; }
        public int UserID { get; set; }
        public int DepartmentID { get; set; }
        public int CreateUserID { get; set; }
        public DateTime CreateDate { get; set; }
        public int ModifyUserID { get; set; }
        public DateTime ModifyDate { get; set; }


    }
}