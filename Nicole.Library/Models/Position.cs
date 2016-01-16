using System;
using Nicole.Library.Models.Interfaces;
using System.Collections.Generic;

namespace Nicole.Library.Models
{
    /// <summary>
    /// 职位表
    /// </summary>
    public class Position : IDtStamped
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual Position Parent { get; set; }
        public virtual ICollection<LeftNavigation> LeftNavigations { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
