using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nicole.Web.Models
{
    public class OrderDetailModel
    {
        public Guid Id { get; set; }
        public EnquiryModel EnquiryModel { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        public decimal? TotalPrice { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal? UnitPrice { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal? Qty { get; set; }
    }
}