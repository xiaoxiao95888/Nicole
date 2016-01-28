using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;

namespace Nicole.Library.Services
{
    public interface IOrderService : IDisposable
    {
        void Insert(Order order);
        void Update();
        void Delete(Guid id);
        Order GetOrder(Guid id);
        IQueryable<Order> GetOrders();
    }
}
