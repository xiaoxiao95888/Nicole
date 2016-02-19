using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nicole.Web.Models
{
    public class ApplyExpenseModel
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public ApplyExpenseTypeModel ApplyExpenseTypeModel { get; set; }
        /// <summary>
        /// 报销人
        /// </summary>
        public PositionModel ConcernedPositionModel { get; set; }
        /// <summary>
        /// 报销日期
        /// </summary>
        public DateTime Date { get; set; }
        public Guid? PositionId { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public PositionModel PositionModel { get; set; }
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