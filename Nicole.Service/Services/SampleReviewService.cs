using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;
using Nicole.Library.Services;

namespace Nicole.Service.Services
{
    public class SampleReviewService: BaseService, ISampleReviewService
    {
        public SampleReviewService(NicoleDataContext dbContext)
            : base(dbContext)
        {
        }

        public void Delete(Guid id)
        {
            var item = GetSampleReview(id);
            if (item != null)
            {
                item.IsDeleted = true;
                Update();
            }
        }

        public SampleReview GetSampleReview(Guid id)
        {
            return DbContext.SampleReviews.FirstOrDefault(n => n.Id == id);
        }

        public IQueryable<SampleReview> GetSampleReviews()
        {
            return DbContext.SampleReviews.Where(n => !n.IsDeleted);
        }

        public void Insert(SampleReview sampleReview)
        {
            DbContext.SampleReviews.Add(sampleReview);
            Update();
        }

        public void Update()
        {
            DbContext.SaveChanges();
        }
    }
}
