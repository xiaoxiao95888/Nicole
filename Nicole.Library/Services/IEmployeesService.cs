using Nicole.Library.Models;
using System;
using System.Linq;

namespace Nicole.Library.Services
{
    public interface IEmployeesService : IDisposable
    {
        void Insert(Employee employee);
        void Update();
        Employee GetEmployee(Guid id);
        IQueryable<Employee> GetEmployees();
        Employee GetReportToEmployee(Employee employee);        
        LeftNavigation GetLeftNavigation(Guid id);
        IQueryable<LeftNavigation> GetLeftNavigations();        
    }
}
