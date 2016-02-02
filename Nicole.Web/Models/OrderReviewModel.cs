using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nicole.Web.Models
{
    public class OrderReviewModel
    {
        public Guid Id { get; set; }
        public Guid? SendToRoleId { get; set; }
        public string ReturnComments { get; set; }
        /// <summary>
        /// 是否退回
        /// </summary>
        public bool IsReturn { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}