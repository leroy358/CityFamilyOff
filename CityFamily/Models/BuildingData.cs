using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class BuildingData
    {
        public int BuildingId { get; set; }
        public string BuildingName { get; set; }
        public string BuildingIndex { get; set; }
        public string BuildingPics { get; set; }
        public string BuildingIntro { get; set; }
        public string BuildingAD { get; set; }
        public string BuildingAroundPic { get; set; }
        public string BuildingDeco { get; set; }
        public string BuildingUpdate { get; set; }
        public List<LayoutData> LayoutData { get; set; }
    }
    public class LayoutData
    {
        public int LayoutId { get; set; }
        public string LayoutName { get; set; }
        public string LayoutPic { get; set; }
        public string LayoutAdvantages { get; set; }
        public string LayoutDisadvantages { get; set; }
        public List<DecorateData> Decotate { get; set; }
        public List<SpotYuanZhuangData> SpotYuanZhuangData { get; set; }
        public List<SpotShiGongData> SpotShiGongData { get; set; }
    }
    public class DecorateData
    {
        public int DecorateId { get; set; }
        public string DecorateIndex { get; set; }
        public string DecoratePics { get; set; }
        public string Decorate360 { get; set; }
    }
    public class SpotYuanZhuangData
    {
        public int SpotId { get; set; }
        public string SpotIndex { get; set; }
        public string SpotPics { get; set; }
    }
    public class SpotShiGongData
    {
        public int SpotId { get; set; }
        public string SpotIndex { get; set; }
        public string SpotPics { get; set; }
    }
}