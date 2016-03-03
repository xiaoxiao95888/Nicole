using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;

namespace Nicole.Library.Services
{
    public interface IOrderDetailService : IDisposable
    {
        void Insert(OrderDetail orderDetail);
        void Update();
        void Delete(Guid id);
        OrderDetail GetOrderDetail(Guid id);
        IQueryable<OrderDetail> GetOrderDetails();
        IQueryable<OrderDetail> GetOrderDetails(Guid orderId);
    }
}
