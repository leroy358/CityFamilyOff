using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class BuildingLayoutView
    {
        public Building building { get; set; }
        public List<Layout> layouts { get; set; }
    }
}