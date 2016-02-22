using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class Admins
    {
        public int Id { get; set; }
        public string AdminName { get; set; }
        public string Password { get; set; }
        public int CompanyID { get; set; }

        public int LoginState { get; set; }
    }
}