using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class BuildingDetails
    {
        public int BuildingId { get; set; }
        public string BuildingName { get; set; }
        public string[] BuildingPics { get; set; }
        public string BuildingIntro { get; set; }
        public string BuildingAD { get; set; }
        public string BuildingAroundPic { get; set; }
        public string BuildingDeco { get; set; }
        public List<BuildingLayout> BuildingLayout { get; set; }

    }
    public class BuildingLayout
    {
        public int LayoutId { get; set; }
        public string LayoutName { get; set; }
        public string LayoutPic { get; set; }
    }
}