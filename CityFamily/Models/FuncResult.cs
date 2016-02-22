using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class FuncResult
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Result { get; set; }
        public bool IsOver { get; set; }

        public int IsLead { get; set; }

        //public string GuestName { get; set; }
        //public string GuestPhone { get; set; }
    }
}