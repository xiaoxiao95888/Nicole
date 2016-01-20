using Nicole.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicole.Library.Services
{
    public interface IPositionCustomerService : IDisposable
    {
        void Insert(PositionCustomer positionCustomer);
        void Update();
        void Delete(Guid id);
        PositionCustomer GetPositionCustomer(Guid id);
        IQueryable<PositionCustomer> GetPositionCustomers();
      
    }
}
