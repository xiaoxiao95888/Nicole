using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;
using Nicole.Library.Services;

namespace Nicole.Service.Services
{
    public class ModeOfPaymentService : BaseService, IModeOfPaymentService
    {
        public ModeOfPaymentService(NicoleDataContext dbContext)
            : base(dbContext)
        {
        }

        public IQueryable<ModeOfPayment> GetModeOfPayments()
        {
            return DbContext.ModeOfPayments;
        }
        
    }
}
