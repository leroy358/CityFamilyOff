using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class UpdateRecord
    {
        public int Id { get; set; }
        public string BuildingId { get; set; }
        public string FurnitureId { get; set; }
        public string StyleId { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}