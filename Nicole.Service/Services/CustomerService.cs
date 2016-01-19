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
            var constant = new[]{
                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U",
                "V", "W", "X", "Y", "Z"};
            var allCode = GetCustomerCodes();
            var rand = new Random();
            while (true)
            {
                var code = constant[rand.Next(0, 25)] + constant[rand.Next(0, 25)] + constant[rand.Next(0, 25)] + constant[rand.Next(0, 25)];
                if (allCode.All(n => n != code))
                {
                    customer.Code = code;
                    DbContext.Customers.Add(customer);
                    Update();
                    break;
                }
            }
        }

        public void Update()
        {
            DbContext.SaveChanges();
        }
    }
}
