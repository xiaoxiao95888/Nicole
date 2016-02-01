using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;

namespace Nicole.Library.Services
{
    public interface IOrderReviewService : IDisposable
    {
        void Insert(OrderReview orderReview);
        void Update();
        void Delete(Guid id);
        OrderReview GetOrderReview(Guid id);
        IQueryable<OrderReview> GetOrderReviews();
    }
}
