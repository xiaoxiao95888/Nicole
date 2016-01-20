using Nicole.Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;

namespace Nicole.Service.Services
{
    public class PositionCustomerService:BaseService, IPositionCustomerService
    {
        public PositionCustomerService(NicoleDataContext dbContext)
             : base(dbContext)
        {
        }

        public void Delete(Guid id)
        {
            var item = GetPositionCustomer(id);
            if (item != null)
            {
                item.IsDeleted = false;
                Update();
            }
        }

        public PositionCustomer GetPositionCustomer(Guid id)
        {
            return DbContext.PositionCustomers.FirstOrDefault(n => n.Id == id);
        }

        public IQueryable<PositionCustomer> GetPositionCustomers()
        {
            return DbContext.PositionCustomers.Where(n => !n.IsDeleted);
        }

        public void Insert(PositionCustomer positionCustomer)
        {
            DbContext.PositionCustomers.Add(positionCustomer);
            Update();
        }

        public void Update()
        {
            DbContext.SaveChanges();
        }
    }
}
