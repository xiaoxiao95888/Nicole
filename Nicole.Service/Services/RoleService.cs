using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;
using Nicole.Library.Services;

namespace Nicole.Service.Services
{
    public class RoleService : BaseService, IRoleService
    {
        public RoleService(NicoleDataContext dbContext)
            : base(dbContext)
        {
        }

        public LeftNavigation GetLeftNavigation(Guid id)
        {
            return DbContext.LeftNavigations.FirstOrDefault(n => n.Id == id);
        }

        public IQueryable<LeftNavigation> GetLeftNavigations()
        {
            return DbContext.LeftNavigations;
        }

        public Role GetRole(Guid id)
        {
            return DbContext.Roles.FirstOrDefault(n => n.Id == id);
        }

        public IQueryable<Role> GetRoles()
        {
            return DbContext.Roles;
        }

        public void Insert(Role role)
        {
            DbContext.Roles.Add(role);
            Update();
        }

        public void Update()
        {
            DbContext.SaveChanges();
        }
    }
}
