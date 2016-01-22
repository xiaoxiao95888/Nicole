using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nicole.Web.MapperHelper.IMapperInterfaces;

namespace Nicole.Web.MapperHelper
{
    public class MapperFactory : IMapperFactory
    {
        public ICustomerMapper GetCustomerMapper()
        {
            return new CustomerMapper();
        }

        public IEmployeeMapper GetEmployeeMapper()
        {
            return new EmployeeMapper();
        }
    }
}