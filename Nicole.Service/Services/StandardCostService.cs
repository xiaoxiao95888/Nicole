using Nicole.Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;

namespace Nicole.Service.Services
{
    public class StandardCostService : BaseService, IStandardCostService
    {
        public StandardCostService(NicoleDataContext dbContext)
            : base(dbContext)
        {
        }

        public void Delete(Guid id)
        {
            var item = GetStandardCost(id);
            if (item != null)
            {
                item.IsDeleted = true;
                Update();
            }
        }

        public IQueryable<StandardCost> GetStandardCosts()
        {
            return DbContext.StandardCosts.Where(n => !n.IsDeleted);
        }

        public StandardCost GetStandardCost(Guid id)
        {
            return DbContext.StandardCosts.FirstOrDefault(n => n.Id == id);
        }

        public void Insert(StandardCost StandardCost)
        {
            DbContext.StandardCosts.Add(StandardCost);
            Update();
        }

        public void Update()
        {
            DbContext.SaveChanges();
        }
    }
}
