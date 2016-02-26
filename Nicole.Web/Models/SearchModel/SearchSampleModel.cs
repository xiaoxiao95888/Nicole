using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nicole.Web.Models.SearchModel
{
    public class SearchSampleModel
    {
        public string Code { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        /// <summary>
        /// 料号
        /// </summary>
        public string PartNumber { get; set; }
        public Guid? PositionId { get; set; }
        public bool? IsApproved { get; set; }
    }
}