using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class SpaceCate
    {
        //Category:
        //1、门厅（玄关）
        //2、过道
        //3、主卧室
        //4、老人房
        //5、儿童房
        //6、厨房
        //7、卫生间
        //8、储物间
        //9、多功能空间
        //10、客厅
        //11、餐厅
        public int Id { get; set; }
        public int Category { get; set; }
        public string SpacePics { get; set; }
    }
}