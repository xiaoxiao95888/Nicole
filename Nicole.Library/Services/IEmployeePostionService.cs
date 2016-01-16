using Nicole.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicole.Library.Services
{
    public interface IEmployeePostionService : IDisposable
    {
        void Insert(EmployeePostion employeePostion);
        void Update();
        void Delete(Guid id);
        EmployeePostion GetEmployeePostion(Guid id);
        IQueryable<EmployeePostion> GeEmployeePostions();        
    }
}
