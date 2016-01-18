using Nicole.Library.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicole.Library.Models
{
    /// <summary>
    /// 订单表(询价)
    /// </summary>
    public class Enquiry : IDtStamped
    {
        public Guid Id { get; set; }
        public Guid? ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public Guid? PositionId { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        [ForeignKey("ProductId")]
        public virtual Position Position { get; set; }
        /// <summary>
        /// 报价
        /// </summary>
        public decimal? Price { get; set; }
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
