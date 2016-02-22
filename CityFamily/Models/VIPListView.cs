using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class VIPListView
    {
        public List<FuncResult> funcResult { get; set; }
        public List<JsonData> jsonData { get; set; }
        public List<DIYResult> diyResult { get; set; }
    }
}