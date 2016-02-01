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

        public IQueryable<string> GetOrderCodes()
        {
            return DbContext.Orders.Select(n => n.Code);
        }

        public IQueryable<Order> GetOrders()
        {
            return DbContext.Orders.Where(n => !n.IsDeleted);
        }

        public void Insert(Order order)
        {
            var maxnumber = 1000;
            var allCode = GetOrderCodes();
            if (allCode.Any())
            {
                maxnumber = Convert.ToInt32(allCode.OrderByDescending(n => n).FirstOrDefault()) + 1;
            }
            order.Code = maxnumber.ToString();
            DbContext.Orders.Add(order);
            Update();
        }

        public void Update()
        {
            DbContext.SaveChanges();
        }
    }
}
