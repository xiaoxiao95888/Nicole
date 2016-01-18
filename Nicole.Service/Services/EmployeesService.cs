using Nicole.Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Library.Models;

namespace Nicole.Service.Services
{
    public class EmployeesService : BaseService, IEmployeesService
    {
        public EmployeesService(NicoleDataContext dbContext)
             : base(dbContext)
        {
        }

        public Employee GetEmployee(Guid id)
        {
            return DbContext.Employees.FirstOrDefault(n => n.Id == id);
        }

        public IQueryable<Employee> GetEmployees()
        {
            return DbContext.Employees.Where(n => !n.IsDeleted);
        }

        public LeftNavigation GetLeftNavigation(Guid id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<LeftNavigation> GetLeftNavigations()
        {
            throw new NotImplementedException();
        }

        public Employee GetReportToEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }

        public void Insert(Employee employee)
        {
            DbContext.Employees.Add(employee);
            Update();
        }

        public void Update()
        {
            DbContext.SaveChanges();
        }
    }
}
