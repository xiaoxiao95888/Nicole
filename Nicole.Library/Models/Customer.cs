using Nicole.Library.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicole.Library.Models
{
    /// <summary>
    /// 客户
    /// </summary>
    public class Customer : IDtStamped
    {
        public Guid Id { get; set; }
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
        public DateTime? UpdateTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
