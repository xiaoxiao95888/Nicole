using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;

namespace Nicole.Library.Services
{
    public interface IRoleService : IDisposable
    {
        void Insert(Role role);
        void Update();
        Role GetRole(Guid id);
        IQueryable<Role> GetRoles();
        LeftNavigation GetLeftNavigation(Guid id);
        IQueryable<LeftNavigation> GetLeftNavigations();
    }
}

