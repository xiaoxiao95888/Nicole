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
    public class ModeOfPaymentController : BaseApiController
    {
        private readonly IModeOfPaymentService _modeOfPaymentService;

        public ModeOfPaymentController(IModeOfPaymentService modeOfPaymentService)
        {
            _modeOfPaymentService = modeOfPaymentService;
        }

        public object Get()
        {
            Mapper.Reset();
            Mapper.CreateMap<ModeOfPayment, ModeOfPaymentModel>();
            return _modeOfPaymentService.GetModeOfPayments().Select(Mapper.Map<ModeOfPayment, ModeOfPaymentModel>);
        }
    }
}
