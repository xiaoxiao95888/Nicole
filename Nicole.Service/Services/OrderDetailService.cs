using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;
using Nicole.Library.Services;

namespace Nicole.Service.Services
{
    public class OrderDetailService : BaseService, IOrderDetailService
    {
        public OrderDetailService(NicoleDataContext dbContext)
            : base(dbContext)
        {
        }

        public void Delete(Guid id)
        {
            var item = GetOrderDetail(id);
            if (item != null)
            {
                item.IsDeleted = true;
                Update();
                
            }
        }

        public OrderDetail GetOrderDetail(Guid id)
        {
            return DbContext.OrderDetails.FirstOrDefault(n => n.Id == id);
        }

        public IQueryable<OrderDetail> GetOrderDetails()
        {
            return DbContext.OrderDetails.Where(n => !n.IsDeleted);
        }

        public IQueryable<OrderDetail> GetOrderDetails(Guid orderId)
        {
            return GetOrderDetails().Where(n => n.OrderId == orderId);
        }

        public void Insert(OrderDetail orderDetail)
        {
            DbContext.OrderDetails.Add(orderDetail);
            Update();
        }

        public void Update()
        {
            DbContext.SaveChanges();
        }
    }
}
