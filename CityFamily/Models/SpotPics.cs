using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class SpotPics
    {
        public int Id { get; set; }
        public string SpotIndex { get; set; }
        public string SpotDetails { get; set; }
        public int LayoutId { get; set; }
        public string CreateTime { get; set; }

        /// <summary>
        /// 添加现场照片分类
        /// category
        /// 1、毛胚照片
        /// 2、施工照片
        /// </summary>
        public int Category { get; set; }
    }
}