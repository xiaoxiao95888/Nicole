using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nicole.Web.Models
{
    public class OrderModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }

        public EnquiryModel EnquiryModel { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        public bool IsApproved { get; set; }
        public string State { get; set; }
        public DateTime OrderDate { get; set; }
        /// <summary>
        /// 最新的审核记录
        /// </summary>
        public OrderReviewModel CurrentOrderReview { get; set; }
        /// <summary>
        /// 账期
        /// </summary>
        public PayPeriodModel PayPeriodModel { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}