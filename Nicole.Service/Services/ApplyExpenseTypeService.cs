using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;
using Nicole.Library.Services;

namespace Nicole.Service.Services
{
    public class ApplyExpenseTypeService : BaseService, IApplyExpenseTypeService
    {
        public ApplyExpenseTypeService(NicoleDataContext dbContext)
             : base(dbContext)
        {
        }

        public ApplyExpenseType GetApplyExpenseType(Guid id)
        {
            return DbContext.ApplyExpenseTypes.FirstOrDefault(n => n.Id == id);
        }

        public IQueryable<ApplyExpenseType> GetApplyExpenseTypes()
        {
            return DbContext.ApplyExpenseTypes;
        }

        public void Update()
        {
            DbContext.SaveChanges();
        }
    }
}
