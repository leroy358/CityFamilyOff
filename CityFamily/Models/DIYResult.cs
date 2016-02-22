using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class DIYResult
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int StyleDetailId { get; set; }
        public string UserName { get; set; }
        public string GuestName { get; set; }
        public string Remark { get; set; }
        public string Style { get; set; }
        public string StyleDetail { get; set; }
        public string DIYJson { get; set; }
        public string CreateTime { get; set; }
        public string StyleUrl { get; set; }
    }
}