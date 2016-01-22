using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;
using Nicole.Library.Services;

namespace Nicole.Service.Services
{
    public class CustomerTypeService: BaseService, ICustomerTypeService
    {
        public CustomerTypeService(NicoleDataContext dbContext)
             : base(dbContext)
        {
        }

        public void Insert(CustomerType customerType)
        {
            DbContext.CustomerTypes.Add(customerType);
            Update();
        }

        public void Update()
        {
            DbContext.SaveChanges();
        }
        public CustomerType GetCustomerType(Guid id)
        {
            return DbContext.CustomerTypes.FirstOrDefault(n => n.Id == id);
        }
        public IQueryable<CustomerType> GetCustomerTypes()
        {
            return DbContext.CustomerTypes;
        }
    }
}
