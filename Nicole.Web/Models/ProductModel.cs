using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nicole.Web.Models
{
    public class ProductModel
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 料号
        /// </summary>
        public string PartNumber { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}