using Nicole.Library.Models;
using Nicole.Library.Services;
using System;
using System.Linq;

namespace Nicole.Service.Services
{
    public class LeftNavigationService : BaseService, ILeftNavigationsService
    {
        public LeftNavigationService(NicoleDataContext dbContext)
            : base(dbContext)
        {
        }


        public LeftNavigation GetLeftNavigation(Guid id)
        {
            return DbContext.LeftNavigations.FirstOrDefault(n => n.Id == id);
        }

        public IQueryable<LeftNavigation> GetLeftNavigations()
        {
            return DbContext.LeftNavigations;
        }


        public LeftNavigation GetLeftNavigation(string url)
        {
            return DbContext.LeftNavigations.FirstOrDefault(n => n.Url == url);
        }
    }
}
