using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class FurnitureStyle
    {
        public int Id { get; set; }
        public string IndexPic { get; set; }
        public string CreateTime { get; set; }
        public string StyleName { get; set; }
        public string FurniturePics { get; set; }
        public string BackPic { get; set; }

        public int StyleId { get; set; }

        public string IdentityId { get; set; }

        public int CreateUserId { get; set; }

        public int CompanyId { get; set; }

        public int FStyleState { get; set; }

    }
}