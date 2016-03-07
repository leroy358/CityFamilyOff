using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class Building
    {
        public int Id { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string BuildingName { get; set; }
        public string BuildingIndex { get; set; }
        public string BuildingPics { get; set; }
        public string BuildingIntro { get; set; }
        public string BuildingAD { get; set; }
        public string BuildingCate { get; set; }
        public string BuildingFeature { get; set; }
        public string BuildingDecorate { get; set; }
        public string CreateTime { get; set; }

        public int CreateUserId { get; set; }

        public int CompanyId { get; set; }
        public DateTime UpdateTime { get; set; }

    }
}