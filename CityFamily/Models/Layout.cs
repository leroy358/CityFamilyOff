using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class Layout
    {
        public int Id { get; set; }
        public string LayoutName { get; set; }
        public string LayoutPic { get; set; }
        public string Advantage { get; set; }
        public string Disadvantage { get; set; }
        public string CadFile { get; set; }
        public int BuildingId { get; set; }
        public string CreateTime { get; set; }
    }
}