using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class T_FurnitureCoverData
    {
        public int FurnitureCoverId { get; set; }
        public string FurnitureCoverName { get; set; }
        public string FurnitureCoverIndex { get; set; }
        public List<FurnitureStyleData> FurnitureStyleData { get; set; }
    }
    public class FurnitureStyleData
    {
        public int FurnitureId { get; set; }
        public string FurnitureIndexPic { get; set; }
        public string FurniturePic { get; set; }
        public string FurnitureBrand { get; set; }
        public string FurnitureSize { get; set; }
        public string FurniturePrize { get; set; }
        public string FurnitureMaterial { get; set; }
    }
}