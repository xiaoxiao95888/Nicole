using Nicole.Library.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nicole.Library.Models
{
    /// <summary>
    /// 客户
    /// </summary>
    public class Customer : IDtStamped
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        public string Email { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string ContactPerson { get; set; }
        public string TelNumber { get; set; }
        public Guid? CustomerTypeId { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        [ForeignKey("CustomerTypeId")]
        public virtual CustomerType CustomerType { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        public string Origin { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public Guid? PositionId { get; set; }
        [ForeignKey("PositionId")]
        public virtual Position Position { get; set; }
        /// <summary>
        /// 分配至角色
        /// </summary>
        public virtual ICollection<PositionCustomer> PositionCustomers { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class CustomerType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
