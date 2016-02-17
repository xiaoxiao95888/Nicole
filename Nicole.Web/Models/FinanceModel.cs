using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nicole.Web.Models
{
    public class FinanceModel
    {
        public Guid Id { get; set; }
        public Guid? OrderId { get; set; }
        /// <summary>
        /// 收款金额
        /// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
        /// 收款日期
        /// </summary>
        public DateTime? PayDate { get; set; }
        /// <summary>
        /// 是否开具发票
        /// </summary>
        public bool HasFaPiao { get; set; }
        public FaPiaoModel[] FaPiaoModels { get; set; }
        public string FaPiaoNumbers { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public PositionModel PositionModel { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsDeleted { get; set; }

    }

    public class FinancePageModel
    {
        public OrderModel OrderModel { get; set; }
        public CustomerModel CustomerModel { get; set; }
        /// <summary>
        /// 实收款
        /// </summary>
        public decimal? RealAmount { get; set; }

        /// <summary>
        /// 收款剩余天数
        /// </summary>
        public int RemainingDays
        {
            get
            {
                var d = OrderModel.LastPayDate - DateTime.Now;
                return d.Days;
            }
        }
    }
    public class FinanceManagerModel
    {
        public FinancePageModel[] Models { get; set; }
        public int CurrentPageIndex { get; set; }
        public int AllPage { get; set; }
    }
}