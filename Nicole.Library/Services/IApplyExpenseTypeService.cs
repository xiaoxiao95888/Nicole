using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;

namespace Nicole.Library.Services
{
    public interface IApplyExpenseTypeService : IDisposable
    {
        void Update();
        ApplyExpenseType GetApplyExpenseType(Guid id);
        IQueryable<ApplyExpenseType> GetApplyExpenseTypes();
    }

}
