using Nicole.Library.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicole.Library.Models
{
    /// <summary>
    /// 人员表
    /// </summary>
    public class Employee : IDtStamped
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime? LeaveDate { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
