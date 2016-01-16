using Nicole.Library.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicole.Library.Models
{
    /// <summary>
    /// 订单表(询价)
    /// </summary>
    public class Order : IDtStamped
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        public virtual Position Position { get; set; }
        /// <summary>
        /// 报价
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 客户
        /// </summary>
        public virtual Customer Customer { get; set; }
        /// <summary>
        /// 是否最终审核通过
        /// </summary>
        public bool IsApprovaled { get; set; }
        /// <summary>
        /// 审核记录
        /// </summary>
        public virtual ICollection<CheckList> CheckList { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
