using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class T_FurnitureCover
    {
        [Key]
        public int Id { get; set; }

        public string StyleName { get; set; }

        public string StylePic { get; set; }

        public DateTime CreateTime { get; set; }

        public int CreateUserId { get; set; }

        public int CompanyId { get; set; }

        public int FStyleState { get; set; }

    }
}