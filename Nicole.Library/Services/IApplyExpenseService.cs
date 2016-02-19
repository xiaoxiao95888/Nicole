using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;

namespace Nicole.Library.Services
{
    public interface IApplyExpenseService : IDisposable
    {
        void Insert(ApplyExpense applyExpense);
        void Update();
        void Delete(Guid id);
        ApplyExpense GetApplyExpense(Guid id);
        IQueryable<ApplyExpense> GetApplyExpenses();
    }
}
