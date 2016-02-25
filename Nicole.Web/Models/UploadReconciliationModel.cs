using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LinqToExcel.Attributes;
using Remotion.Data.Linq.Parsing.ExpressionTreeVisitors.MemberBindings;

namespace Nicole.Web.Models
{
    public class UploadReconciliationModel
    {
        [ExcelColumn("完整料号")]
        public string PartNumber { get; set; }
        [ExcelColumn("成本")]
        public decimal Price { get; set; }
        [ExcelColumn("数量")]
        public decimal Qty { get; set; }
        [ExcelColumn("订单号")]
        public string OrderCode { get; set; }
        public string State { get; set; }
        public DateTime? OrderDate { get; set; }
    }
}