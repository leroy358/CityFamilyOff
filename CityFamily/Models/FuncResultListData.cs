using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class FuncResultListData
    {
        public List<FuncResultStateOne> FuncResultStateOne { get; set; }
        public List<FuncResultStateTwo> FuncResultStateTwo { get; set; }
        public List<FuncResultStateThree> FuncResultStateThree { get; set; }
    }
    public class FuncResultStateOne
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string XiaoQu { get; set; }
        public string Usage { get; set; }
        public string Work { get; set; }
    }
    public class FuncResultStateTwo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string XiaoQu { get; set; }
        public string Usage { get; set; }
        public string Work { get; set; }
    }
    public class FuncResultStateThree
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string XiaoQu { get; set; }
        public string Usage { get; set; }
        public string Work { get; set; }
    }
}