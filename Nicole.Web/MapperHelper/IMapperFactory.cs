using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicole.Web.MapperHelper.IMapperInterfaces;

namespace Nicole.Web.MapperHelper
{
    public interface IMapperFactory
    {
        ICustomerMapper GetCustomerMapper();
        IEmployeeMapper GetEmployeeMapper();
        IStandardCostMapper GetStandardCostMapper();
        IEnquiryMapper GetEnquiryMapper();
        IOrderMapper GetOrderMapper();
        IFinanceMapper GetFinanceMapper();
        IApplyExpenseMapper GetApplyExpenseMapper();
    }
}
