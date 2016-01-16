using Nicole.Library.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicole.Library.Models
{
    public class Product: IDtStamped
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 料号
        /// </summary>
        public string PartNumber { get; set; }
        public virtual ProductType ProductType { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
