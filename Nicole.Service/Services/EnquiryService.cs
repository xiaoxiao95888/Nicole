using Nicole.Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;

namespace Nicole.Service.Services
{
    public class EnquiryService : BaseService, IEnquiryService
    {
        public EnquiryService(NicoleDataContext dbContext)
             : base(dbContext)
        {
        }

        public void Delete(Guid id)
        {
            var item = GetEnquiry(id);
            if (item != null)
            {
                item.IsDeleted = true;
                Update();
            }
        }

        public IQueryable<Enquiry> GetEnquiries()
        {
            return DbContext.Enquiries.Where(n => !n.IsDeleted);
        }

        public Enquiry GetEnquiry(Guid id)
        {
            return DbContext.Enquiries.FirstOrDefault(n => n.Id == id);
        }

        public void Insert(Enquiry enquiry)
        {
            DbContext.Enquiries.Add(enquiry);
            Update();
        }

        public void Update()
        {
            DbContext.SaveChanges();
        }
    }
}
