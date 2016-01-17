using System;
using System.Linq;

namespace Nicole.Web.Models
{
    public class ProductModel
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 型号
        /// </summary>
        public string ProductType { get; set; }
        /// <summary>
        /// 电压
        /// </summary>
        public string Voltage { get; set; }
        /// <summary>
        /// 容量
        /// </summary>
        public string Capacity { get; set; }
        /// <summary>
        /// 脚距
        /// </summary>
        public string Pitch { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// 料号
        /// </summary>
        public string PartNumber { get; set; }
        /// <summary>
        /// 特殊设计
        /// </summary>
        public string SpecificDesign { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}