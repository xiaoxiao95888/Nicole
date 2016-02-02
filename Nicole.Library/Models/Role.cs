using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicole.Library.Models
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Position> Positions { get; set; }
        public virtual ICollection<LeftNavigation> LeftNavigations { get; set; }
    }
    
}
