using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;
using Nicole.Library.Services;

namespace Nicole.Service.Services
{
    public class AuditLevelService : BaseService, IAuditLevelService
    {
        public AuditLevelService(NicoleDataContext dbContext)
             : base(dbContext)
        {
        }

        public AuditLevel GetAuditLevel(Guid id)
        {
            return DbContext.AuditLevels.FirstOrDefault(n => n.Id == id);
        }

        public IQueryable<AuditLevel> GetAuditLevels()
        {
            return DbContext.AuditLevels;
        }
    }
}
