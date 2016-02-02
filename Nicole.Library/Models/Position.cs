using System;
using Nicole.Library.Models.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nicole.Library.Models
{
    /// <summary>
    /// 职位表
    /// </summary>
    public class Position : IDtStamped
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
        public virtual Position Parent { get; set; }
        public virtual ICollection<EmployeePostion> EmployeePostions { get; set; }
        public virtual ICollection<PositionCustomer> PositionCustomers { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
