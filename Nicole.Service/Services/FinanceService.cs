using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;
using Nicole.Library.Services;

namespace Nicole.Service.Services
{
    public class FinanceService : BaseService, IFinanceService
    {
        public FinanceService(NicoleDataContext dbContext)
            : base(dbContext)
        {
        }

        public void Delete(Guid id)
        {
            var item = GetFinance(id);
            if (item != null)
            {
                item.IsDeleted = true;
                Update();
            }
        }

        public Finance GetFinance(Guid id)
        {
            return DbContext.Finances.FirstOrDefault(n => n.Id == id);
        }

        public IQueryable<Finance> GetFinances()
        {
            return DbContext.Finances.Where(n => !n.IsDeleted);
        }

        public void Insert(Finance finance)
        {
            DbContext.Finances.Add(finance);
            Update();
        }

        public void Update()
        {
            DbContext.SaveChanges();
        }
    }
}
