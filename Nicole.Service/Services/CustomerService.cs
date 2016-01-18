using Nicole.Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;

namespace Nicole.Service.Services
{
    public class CustomerService : BaseService, ICustomerService
    {
        public CustomerService(NicoleDataContext dbContext)
             : base(dbContext)
        {
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Customer GetCustomer(Guid id)
        {
            return DbContext.Customers.FirstOrDefault(n => n.Id == id);
        }

        public IQueryable<Customer> GetCustomers()
        {
            return DbContext.Customers.Where(n => !n.IsDeleted);
        }

        public void Insert(Customer customer)
        {
            DbContext.Customers.Add(customer);
            Update();           
        }

        public void Update()
        {
            DbContext.SaveChanges();
        }
    }
}
