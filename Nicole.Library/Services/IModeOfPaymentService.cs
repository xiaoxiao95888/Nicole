using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;

namespace Nicole.Library.Services
{
    public interface IModeOfPaymentService : IDisposable
    {
        IQueryable<ModeOfPayment> GetModeOfPayments();
    }

}
