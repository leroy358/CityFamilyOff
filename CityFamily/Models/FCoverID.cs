using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class FCoverID
    {

        [Key]
        public int Id { get; set; }

        public int FCoverId { get; set; }

        public int CompanyId { get; set; }

        public DateTime CreateTime { get; set; }

        public int CreateUserId { get; set; }

    }
}