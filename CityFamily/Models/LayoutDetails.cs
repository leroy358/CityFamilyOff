using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class LayoutDetails
    {

        public int LayoutId { get; set; }
        public string LayoutName { get; set; }
        public string LayoutPic { get; set; }
        public string LayoutAdvantages { get; set; }
        public string LayoutDisadvantages { get; set; }
        public List<LayoutDecorate> Decotate { get; set; }
        public List<LayoutSpotYuanZhuang> SpotYuanZhuangData { get; set; }
        public List<LayoutSpotShiGong> SpotShiGongData { get; set; }


    }
    public class LayoutDecorate
    {
        public int DecorateId { get; set; }
        public string DecorateIndex { get; set; }
        public int Is360 { get; set; }
    }
    public class LayoutSpotYuanZhuang
    {
        public int SpotId { get; set; }
        public string SpotIndex { get; set; }
        public string SpotPics { get; set; }
    }
    public class LayoutSpotShiGong
    {
        public int SpotId { get; set; }
        public string SpotIndex { get; set; }
        public string SpotPics { get; set; }
    }
}