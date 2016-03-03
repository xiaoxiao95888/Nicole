using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Nicole.Library.Models.Interfaces;

namespace Nicole.Library.Models
{
    /// <summary>
    /// 订单
    /// </summary>
    public class Order : IDtStamped
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        /// <summary>
        /// 合同金额
        /// </summary>
        public decimal ContractAmount { get; set; }
        /// <summary>
        /// 合同详细
        /// </summary>
        public virtual ICollection<OrderDetail>  OrderDetails { get; set; }
        public virtual ICollection<OrderReview> OrderReviews { get; set; }
        public virtual ICollection<Finance> Finances { get; set; }
        /// <summary>
        /// 合同日期
        /// </summary>
        public DateTime OrderDate { get; set; }
        /// <summary>
        /// 预计交货日期
        /// </summary>
        public string EstimatedDeliveryDate { get; set; }

        public Guid? PayPeriodId { get; set; }
        /// <summary>
        /// 账期
        /// </summary>
        [ForeignKey("PayPeriodId")]
        public virtual PayPeriod PayPeriod { get; set; }
        /// <summary>
        /// 截止收款日
        /// </summary>
        public DateTime LastPayDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        public bool IsApproved { get; set; }
        public Guid? PositionId { get; set; }
        /// <summary>
        /// 合同申请人
        /// </summary>
        [ForeignKey("PositionId")]
        public virtual Position Position { get; set; }
        public Guid? CustomerId { get; set; }
        /// <summary>
        /// 客户
        /// </summary>
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
