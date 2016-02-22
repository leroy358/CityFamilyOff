using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class FurnitureResult
    {
        /// <summary>
        /// 新增家具图片保存模型，
        /// 编号，用户编码，图片页码
        /// </summary>
        public int Id { get; set; }
        public string CustomerCode { get; set; }
        public int picIndex { get; set; }

    }
}