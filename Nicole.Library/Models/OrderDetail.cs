using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models.Interfaces;

namespace Nicole.Library.Models
{
    public class OrderDetail : IDtStamped
    {
        public Guid Id { get; set; }
        public Guid? OrderId { get; set; }
        public Guid? EnquiryId { get; set; }
        /// <summary>
        /// 合同Id
        /// </summary>
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
        /// <summary>
        /// 询价
        /// </summary>
        [ForeignKey("EnquiryId")]
        public virtual Enquiry Enquiry { get; set; }
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
        public DateTime? UpdateTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
