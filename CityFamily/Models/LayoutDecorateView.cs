using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class LayoutDecorateView
    {
        public Layout layout { get; set; }
        public List<Decorate> decorates { get; set; }
        public List<SpotPics> spotPics { get; set; }
    }
}