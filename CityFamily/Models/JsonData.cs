using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CityFamily.Models
{
    public class JsonData
    {
        public string Name { get; set; }
        public string Age { get; set; }
        public List<Usage> Usage { get; set; }
        public List<Work> Work { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string XiaoQu { get; set; }
        public List<Anniversary> Anniversary { get; set; }
        public List<GoodsHad> GoodsHad { get; set; }
        public List<Family> Family { get; set; }
        public string AgreedTime { get; set; }
        public List<Equipment> Equipment { get; set; }
        public List<Intrest> Intrest { get; set; }
        public List<Material> Material { get; set; }
        /// <summary>
        /// 添加MateriaNo
        /// </summary>
        public List<MaterialNo> MaterialNo { get; set; }
        /// <summary>
        /// ================2015-9-6 添加MateriaOther
        /// </summary>
        public string MaterialOther { get; set; }
        public string FurnitureViewUrl { get; set; }
        public string diyResultUrl { get; set; }
        public List<Space> Space { get; set; }
    }
    public class Usage
    {
        public string Name { get; set; }
    }
    public class Work
    {
        public string Name { get; set; }
        /// <summary>
        /// 添加Other
        /// </summary>
        public string Other { get; set; }
    }
    public class Anniversary
    {
        public string Name { get; set; }
        public string Date { get; set; }
    }
    public class GoodsHad
    {
        public string Name { get; set; }
        public string Size { get; set; }
        public string Position { get; set; }
    }
    public class Family
    {
        public string Name { get; set; }
        public string Age { get; set; }
        public string Work { get; set; }
        public string Hobby { get; set; }
        //public string Other { get; set; }

        /// <summary>
        /// 添加生日选项
        /// </summary>
        public string Birthday { get; set; }
    }
    public class Equipment
    {
        public string Name { get; set; }
    }
    public class Intrest
    {
        public string Name { get; set; }
        /// <summary>
        /// 添加Other
        /// </summary>
        public string Other { get; set; }
    }
    public class Material
    {
        public string Name { get; set; }
    }
    public class MaterialNo
    {
        public string Name { get; set; }
    }
    public class Space
    {
        public string Name { get; set; }
        //public List<Options> Options { get; set; }
        /// <summary>
        /// 添加Other，删除Options
        /// </summary>
        public string Other { get; set; }
    }
    //public class Options
    //{
    //    public string Name { get; set; }
    //}
}