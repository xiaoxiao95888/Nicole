using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models.Interfaces;

namespace Nicole.Library.Models
{
    public class FaPiao: IDtStamped
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public Guid? FinanceId { get; set; }
        public virtual Finance Finance { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
