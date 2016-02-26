using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;
using Nicole.Library.Services;

namespace Nicole.Service.Services
{
    public class SampleService : BaseService, ISampleService
    {
        public SampleService(NicoleDataContext dbContext)
            : base(dbContext)
        {
        }
        public IQueryable<string> GetSampleCodes()
        {
            return DbContext.Samples.Select(n => n.Code);
        }
        public void Delete(Guid id)
        {
            var item = GetSample(id);
            if (item != null)
            {
                item.IsDeleted = false;
                Update();
            }
        }

        public Sample GetSample(Guid id)
        {
            return DbContext.Samples.FirstOrDefault(n => n.Id == id);
        }

        public IQueryable<Sample> GetSamples()
        {
            return DbContext.Samples.Where(n => !n.IsDeleted);
        }

        public void Insert(Sample sample)
        {
            var maxnumber = 1000;
            var allCode = GetSampleCodes();
            if (allCode.Any())
            {
                maxnumber = Convert.ToInt32(allCode.OrderByDescending(n => n).FirstOrDefault()) + 1;
            }
            sample.Code = maxnumber.ToString();
            DbContext.Samples.Add(sample);
            Update();
        }

        public void Update()
        {
            DbContext.SaveChanges();
        }
    }
}
