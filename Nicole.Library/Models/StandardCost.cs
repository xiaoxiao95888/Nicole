using Nicole.Library.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicole.Library.Models
{
    /// <summary>
    /// 总经理设定的标准报价
    /// </summary>
    public class StandardCost : IDtStamped
    {
        public Guid Id { get; set; }

        public Guid? ProductId { get; set; }
        /// <summary>
        /// 产品
        /// </summary>
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        /// <summary>
        /// 报价
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 报价时间
        /// </summary>
        public DateTime QuotedTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
