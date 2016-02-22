using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class T_AuthInfo
    {
        [Key]
        public int AuthID { get; set; }
        public string AuthMaster { get; set; }
        public int AuthMasterValue { get; set; }

        public string AuthAccess { get; set; }
        public int AuthAccessValue { get; set; }
        public int AuthOperation { get; set; }
        public int CreateUserID { get; set; }
        public DateTime CreateDate { get; set; }
        public int ModifyUserID { get; set; }
        public DateTime ModifyDate { get; set; }

        public string ModifyRemark { get; set; }

    }
}