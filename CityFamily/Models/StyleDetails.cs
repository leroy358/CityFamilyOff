using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class StyleDetails
    {
        public int Id { get; set; }
        public string StyleDetailName { get; set; }
        public string StyleDetailIndex { get; set; }
        //public string StyleDetailPics { get; set; }
        //public string styleDetail720 { get; set; }
        //public string StyleDetailCode { get; set; }
        //public string StyleResource { get; set; }
        public int StyleId { get; set; }
        public string CreateTime { get; set; }
    }
}