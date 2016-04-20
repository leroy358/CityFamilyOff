using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class StyleAllData
    {
        public int Id { get; set; }
        public string StyleName { get; set; }
        public string StyleIndex { get; set; }
        public string UpdateTime { get; set; }
        public List<StyleSecondData> StyleSecondDataList { get; set; }
    }
    public class StyleSecondData
    {
        public int StyleSecondId { get; set; }
        public string StyleSecondName { get; set; }
        public string StyleSecondIndex { get; set; }
        public List<StyleThirdDownData> StyleThirdDatsList { get; set; }
    }
    
}