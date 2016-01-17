using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nicole.Web.Models
{
    public class StandardCostModel
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 产品
        /// </summary>
        public ProductModel ProductModel { get; set; }
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