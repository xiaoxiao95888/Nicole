using Nicole.Library.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicole.Library.Models
{
    public class PositionCustomer : IDtStamped
    {
        public Guid Id { get; set; }
        public Guid? PositionId { get; set; }
        public Guid? CustomerId { get; set; }
        [ForeignKey("PositionId")]
        public virtual Position Position { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
