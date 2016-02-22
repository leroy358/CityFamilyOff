using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class FurnitureStylePics
    {
        public int Id { get; set; }
        public string StyleName { get; set; }

        public string BackPic { get; set; }

        public int StyleId { get; set; }

        public List<T_StyleFurniturePics> T_StyleFurniturePicsList { get; set; }

    }
}