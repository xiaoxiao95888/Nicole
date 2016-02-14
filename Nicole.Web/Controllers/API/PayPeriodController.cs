using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Nicole.Library.Models;
using Nicole.Library.Services;
using Nicole.Web.Models;

namespace Nicole.Web.Controllers.API
{
    public class PayPeriodController : BaseApiController
    {
        private readonly IPayPeriodService _payPeriodService;

        public PayPeriodController(IPayPeriodService payPeriodService)
        {
            _payPeriodService = payPeriodService;
        }

        public object Get()
        {
            Mapper.Reset();
            Mapper.CreateMap<PayPeriod, PayPeriodModel>();
            return _payPeriodService.GetPayPeriods().Select(Mapper.Map<PayPeriod, PayPeriodModel>);
        }
    }
}
