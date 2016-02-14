using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;
using Nicole.Library.Services;

namespace Nicole.Service.Services
{
    public class PayPeriodService : BaseService, IPayPeriodService
    {
        public PayPeriodService(NicoleDataContext dbContext) : base(dbContext)
        {
        }
        public IQueryable<PayPeriod> GetPayPeriods()
        {
            return DbContext.PayPeriods;
        }

       
    }
}
