using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;

namespace Nicole.Library.Services
{
    public interface ISampleReviewService : IDisposable
    {
        void Insert(SampleReview sampleReview);
        void Update();
        void Delete(Guid id);
        SampleReview GetSampleReview(Guid id);
        IQueryable<SampleReview> GetSampleReviews();
    }
}
