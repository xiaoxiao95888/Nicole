using Nicole.Library.Models;
using System;
using System.Linq;

namespace Nicole.Library.Services
{
    public interface ILeftNavigationsService : IDisposable
    {
        LeftNavigation GetLeftNavigation(Guid id);
        IQueryable<LeftNavigation> GetLeftNavigations();
        LeftNavigation GetLeftNavigation(string url);
    }
}
