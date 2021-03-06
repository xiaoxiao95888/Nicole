﻿using System;
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

        public IEnquiryMapper GetEnquiryMapper()
        {
           return new EnquiryMapper();
        }

        public IStandardCostMapper GetStandardCostMapper()
        {
            return new StandardCostMapper();
        }
        public IOrderMapper GetOrderMapper()
        {
            return new OrderMapper();
        }

        public IFinanceMapper GetFinanceMapper()
        {
            return new FinanceMapper();
        }

        public IApplyExpenseMapper GetApplyExpenseMapper()
        {
            return new ApplyExpenseMapper();
        }

        public ISampleMapper GetSampleMapper()
        {
            return new SampleMapper();
        }
    }
}