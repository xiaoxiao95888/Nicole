using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models.Interfaces;

namespace Nicole.Library.Models
{
    /// <summary>
    /// 样品
    /// </summary>
    public class Sample : IDtStamped
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public Guid? CustomerId { get; set; }
        /// <summary>
        /// 客户
        /// </summary>
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        public Guid? ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public Guid? PositionId { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        [ForeignKey("PositionId")]
        public virtual Position Position { get; set; }
        public decimal Qty { get; set; }
        public virtual ICollection<SampleReview> SampleReviews { get; set; }
        public bool IsApproved { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
