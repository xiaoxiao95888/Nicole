using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;
using Nicole.Library.Services;

namespace Nicole.Service.Services
{
    public class ApplyExpenseService : BaseService, IApplyExpenseService
    {
        public ApplyExpenseService(NicoleDataContext dbContext)
             : base(dbContext)
        {
        }

        public void Delete(Guid id)
        {
            var item = GetApplyExpense(id);
            if (item != null)
            {
                item.IsDeleted = true;
                Update();
            }
        }

        public ApplyExpense GetApplyExpense(Guid id)
        {
            return DbContext.ApplyExpenses.FirstOrDefault(n => n.Id == id);
        }

        public IQueryable<ApplyExpense> GetApplyExpenses()
        {
            return DbContext.ApplyExpenses.Where(n => !n.IsDeleted);
        }

        public void Insert(ApplyExpense applyExpense)
        {
            DbContext.ApplyExpenses.Add(applyExpense);
            Update();
        }

        public void Update()
        {
            DbContext.SaveChanges();
        }
    }
}
