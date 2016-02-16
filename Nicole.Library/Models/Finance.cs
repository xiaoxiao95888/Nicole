using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models.Interfaces;

namespace Nicole.Library.Models
{
    public class Finance : IDtStamped
    {
        public Guid Id { get; set; }
        public Guid? OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
        /// <summary>
        /// 收款金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 收款日期
        /// </summary>
        public DateTime PayDate { get; set; }
        /// <summary>
        /// 是否开具发票
        /// </summary>
        public bool HasFaPiao { get; set; }
        /// <summary>
        /// 发票编号
        /// </summary>
        public string FaPiaoNumber { get; set; }
        public Guid? PositionId { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        [ForeignKey("PositionId")]
        public virtual Position Position { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
