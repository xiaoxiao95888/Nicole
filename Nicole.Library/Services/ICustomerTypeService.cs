using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;

namespace Nicole.Library.Services
{
    public interface ICustomerTypeService : IDisposable
    {
        void Insert(CustomerType customerType);
        void Update();
        void Delete(Guid id);
        CustomerType GetCustomerType(Guid id);
        IQueryable<CustomerType> GetCustomerTypes();
        
    }
}
