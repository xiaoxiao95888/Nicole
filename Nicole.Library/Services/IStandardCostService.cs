using Nicole.Library.Models;
using System;
using System.Linq;

namespace Nicole.Library.Services
{
    public interface IStandardCostService : IDisposable
    {
        void Insert(StandardCost standardCost);
        void Update();
        void Delete(Guid id);
        StandardCost GetStandardCost(Guid id);
        IQueryable<StandardCost> GetStandardCosts();
    }
}
