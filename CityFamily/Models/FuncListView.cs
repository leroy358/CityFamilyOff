using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class FuncListView
    {
        public List<FuncResult> FuncResult { get; set; }
        public List<Admins> User { get; set; }
        public List<JsonData> DataResult { get; set; }
    }
}