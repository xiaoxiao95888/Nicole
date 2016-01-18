using Nicole.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicole.Library.Services
{
    public interface IEnquiryService : IDisposable
    {
        void Insert(Enquiry enquiry);
        void Update();
        void Delete(Guid id);
        Enquiry GetEnquiry(Guid id);
        IQueryable<Enquiry> GetEnquiries();
    }
}
