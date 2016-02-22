using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class Styles
    {
        public int Id { get; set; }
        public string StyleName { get; set; }
        public string StyleIndex { get; set; }
        public string CreateTime { get; set; }
        public int CreateUserId { get; set; }
        public int CompanyId { get; set; }

        public int FStyleState { get; set; }
    }
}