using Nicole.Library.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicole.Library.Models
{
    /// <summary>
    /// 总经理设定的标准报价
    /// </summary>
    public class StandardCost : IDtStamped
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 完整的料号
        /// </summary>
        public virtual Product Product { get; set; }
        /// <summary>
        /// 特殊设计
        /// </summary>
        public string SpecificDesign { get; set; }
        /// <summary>
        /// 成本
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 报价时间
        /// </summary>
        public DateTime QuotedTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
