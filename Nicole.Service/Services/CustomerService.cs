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
            var item = GetCustomer(id);
            if (item != null)
            {
                item.IsDeleted = true;
                Update();
            }
        }

        public Customer GetCustomer(Guid id)
        {
            return DbContext.Customers.FirstOrDefault(n => n.Id == id);
        }

        public IQueryable<string> GetCustomerCodes()
        {
            return DbContext.Customers.Select(n => n.Code);
        }

        public IQueryable<Customer> GetCustomers()
        {
            return DbContext.Customers.Where(n => !n.IsDeleted);
        }

        public void Insert(Customer customer)
        {
            var maxnumber = 1000;
            var allCode = GetCustomerCodes();
            if (allCode.Any())
            {
                maxnumber = Convert.ToInt32(allCode.OrderByDescending(n => n).FirstOrDefault()) + 1;
            }
            customer.Code = maxnumber.ToString();
            DbContext.Customers.Add(customer);
            Update();
        }

        public void Update()
        {
            DbContext.SaveChanges();
        }
    }
}
