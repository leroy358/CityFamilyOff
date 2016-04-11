using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class FuncResultDetailData
    {
        public List<FuncFuWu> FuncFuWuList { get; set; }
        public List<FuncLiangFang> FuncLiangFangList { get; set; }
        public List<FuncQianYue> FuncQianYueList { get; set; }

    }
    public class FuncFuWu
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Result { get; set; }
        public int IsLead { get; set; }
    }
    public class FuncLiangFang
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Result { get; set; }
        public int IsLead { get; set; }
    }
    public class FuncQianYue
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Result { get; set; }
        public int IsLead { get; set; }
    }
}