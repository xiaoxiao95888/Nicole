using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nicole.Web.Models
{
    public class SampleModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public PositionModel PositionModel { get; set; }
        public CustomerModel CustomerModel { get; set; }
        public ProductModel ProductModel { get; set; }
        public decimal? Qty { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public string State { get; set; }
        public bool IsApproved { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime CreatedTime { get; set; }
    }
    public class SampleSettingModel
    {
        public SampleModel[] Models { get; set; }
        public int CurrentPageIndex { get; set; }
        public int AllPage { get; set; }
    }
}