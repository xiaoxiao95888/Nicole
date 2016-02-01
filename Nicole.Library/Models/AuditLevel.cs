using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicole.Library.Models
{
    public class AuditLevel
    {
        public Guid Id { get; set; }
        public Guid? RoleId { get; set; }
        public Guid? ParentRoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual  Role Role { get; set; }
        [ForeignKey("ParentRoleId")]
        public virtual Role ParentRole { get; set; }
    }
}
