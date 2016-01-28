using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;
using Nicole.Library.Services;

namespace Nicole.Service.Services
{
    public class OrderService : BaseService, IOrderService
    {
        public OrderService(NicoleDataContext dbContext)
            : base(dbContext)
        {
        }

        public void Delete(Guid id)
        {
            var item = GetOrder(id);
            if (item != null)
            {
                item.IsDeleted = true;
                Update();
            }
        }

        public Order GetOrder(Guid id)
        {
            return DbContext.Orders.FirstOrDefault(n => n.Id == id);
        }

        public IQueryable<Order> GetOrders()
        {
            return DbContext.Orders.Where(n => !n.IsDeleted);
        }

        public void Insert(Order order)
        {
            DbContext.Orders.Add(order);
            Update();
        }

        public void Update()
        {
            DbContext.SaveChanges();
        }
    }
}
