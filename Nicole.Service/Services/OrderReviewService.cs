using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;
using Nicole.Library.Services;

namespace Nicole.Service.Services
{
    public class OrderReviewService : BaseService, IOrderReviewService
    {
        public OrderReviewService(NicoleDataContext dbContext)
            : base(dbContext)
        {
        }

        public void Delete(Guid id)
        {
            var item = GetOrderReview(id);
            if (item != null)
            {
                item.IsDeleted = true;
                Update();
            }
        }

        public OrderReview GetOrderReview(Guid id)
        {
            return DbContext.OrderReviews.FirstOrDefault(n => n.Id == id);
        }

        public IQueryable<OrderReview> GetOrderReviews()
        {
            return DbContext.OrderReviews.Where(n => !n.IsDeleted);
        }

        public void Insert(OrderReview orderReview)
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            DbContext.SaveChanges();
        }
    }
}
