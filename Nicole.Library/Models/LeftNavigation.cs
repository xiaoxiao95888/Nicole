using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicole.Library.Models
{
    public class LeftNavigation
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }       
        public virtual LeftNavigation Parent { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}
