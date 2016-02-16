using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;

namespace Nicole.Library.Services
{
    public interface IPayPeriodService : IDisposable
    {
        PayPeriod GetPayPeriod(Guid id);
        IQueryable<PayPeriod> GetPayPeriods();
    }
}
