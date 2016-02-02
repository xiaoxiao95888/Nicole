using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models.Interfaces;

namespace Nicole.Library.Models
{
    public class OrderReview: IDtStamped
    {
        public Guid? Id { get; set; }
        public Guid? OrderId { get; set; }
        /// <summary>
        /// 等待审核的订单
        /// </summary>
        [ForeignKey("OrderId")]
        public virtual Order Orders { get; set; }
        public Guid? SendToRoleId { get; set; }
        /// <summary>
        /// 发送至审核角色
        /// </summary>
        [ForeignKey("SendToRoleId")]
        public virtual Role SendToRole { get; set; }
        /// <summary>
        /// 退回原因
        /// </summary>
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
