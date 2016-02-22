using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class FStyleID
    {
        [Key]
        public int Id { get; set; }

        public int FStyleId { get; set; }

        public int CompanyId { get; set; }

    }
}