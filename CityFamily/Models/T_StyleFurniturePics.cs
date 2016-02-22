using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class T_StyleFurniturePics
    {
        [Key]
        public int Id { get; set; }

        public int StyleId { get; set; }

        public string FurniturePics { get; set; }

        public DateTime CreateTime { get; set; }

        public string IdentityId { get; set; }
    }
}