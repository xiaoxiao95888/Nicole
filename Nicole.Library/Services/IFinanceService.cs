using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;

namespace Nicole.Library.Services
{
    public interface IFinanceService : IDisposable
    {
        void Insert(Finance finance);
        void Update();
        void Delete(Guid id);
        Finance GetFinance(Guid id);
        IQueryable<Finance> GetFinances();
    }
}
