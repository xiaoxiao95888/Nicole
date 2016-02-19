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
    /// 报销
    /// </summary>
    public class ApplyExpense : IDtStamped
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public Guid? ApplyExpenseTypeId { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        [ForeignKey("ApplyExpenseTypeId")]
        public virtual ApplyExpenseType ApplyExpenseType { get; set; }
        /// <summary>
        /// 报销日期
        /// </summary>
        public DateTime Date { get; set; }
        public Guid? ConcernedPositionId { get; set; }
        /// <summary>
        /// 报销人
        /// </summary>
        [ForeignKey("ConcernedPositionId")]
        public virtual Position ConcernedPosition { get; set; }
        public Guid? PositionId { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        [ForeignKey("PositionId")]
        public virtual Position Position { get; set; }
        /// <summary>
        /// 报销明细
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// 是否通过申请
        /// </summary>
        public bool IsApproved { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
