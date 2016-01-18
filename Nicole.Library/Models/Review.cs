using Nicole.Library.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicole.Library.Models
{
    public class Review : IDtStamped
    {
        public Guid Id { get; set; }
        //public virtual Enquiry Order { get; set; }
        /// <summary>
        /// 当前审核环节的审核人
        /// </summary>
        public virtual Position Position { get; set; }
        /// <summary>
        /// 是否退回
        /// </summary>
        public bool IsReturn { get; set; }
        public string Remark { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
