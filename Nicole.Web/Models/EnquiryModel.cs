using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nicole.Web.Models
{
    public class EnquiryModel
    {
        public Guid Id { get; set; }
        public ProductModel ProductModel { get; set; }        
        /// <summary>
        /// 申请人
        /// </summary>
        public PositionModel PositionModel { get; set; }
        /// <summary>
        /// 报价
        /// </summary>
        public decimal? Price { get; set; }
        /// <summary>
        /// 客户
        /// </summary>
        public CustomerModel CustomerModel { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}