using Nicole.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicole.Library.Services
{
    public interface ICustomerService : IDisposable
    {
        void Insert(Customer customer);
        void Update();
        void Delete(Guid id);
        Customer GetCustomer(Guid id);
        IQueryable<Customer> GetCustomers();
        IQueryable<string> GetCustomerCodes();
    }
}
