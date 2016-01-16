using Nicole.Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;

namespace Nicole.Service.Services
{
    public class EmployeePostionService : BaseService, IEmployeePostionService
    {
        public EmployeePostionService(NicoleDataContext dbContext)
             : base(dbContext)
        {
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<EmployeePostion> GeEmployeePostions()
        {
            return DbContext.EmployeePostions.Where(n => !n.IsDeleted);
        }

        public EmployeePostion GetEmployeePostion(Guid id)
        {
            return DbContext.EmployeePostions.FirstOrDefault(n => n.Id == id);
        }

        public void Insert(EmployeePostion employeePostion)
        {
            DbContext.EmployeePostions.Add(employeePostion);
            DbContext.SaveChanges();
        }

        public void Update()
        {
            DbContext.SaveChanges();
        }
    }
}
