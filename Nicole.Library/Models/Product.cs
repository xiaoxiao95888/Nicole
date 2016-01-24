using Nicole.Library.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicole.Library.Models
{
    public class Product: IDtStamped
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 料号
        /// </summary>
        public string PartNumber { get; set; }
        /// <summary>
        /// 型号
        /// </summary>
        public string  ProductType  { get; set; }
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
        /// 特殊设计
        /// </summary>
        public string SpecificDesign { get; set; }
        /// <summary>
        /// 成本
        /// </summary>
        public decimal? Price { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
