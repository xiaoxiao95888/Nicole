using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nicole.Web.Models.SearchModel
{
    public class SearchApplyExpenseModel
    {
        public DateRangeModel DateRangeModel { get; set; }
        public ApplyExpenseTypeModel ApplyExpenseTypeModel { get; set; }
        public PositionModel ConcernedPositionModel { get; set; }
        public string Detail { get; set; }
        public bool? IsApproved { get; set; }
    }
}