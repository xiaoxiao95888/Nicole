using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Nicole.Library.Models;
using Nicole.Library.Services;
using Nicole.Web.MapperHelper;
using Nicole.Web.Models;

namespace Nicole.Web.Controllers.API
{
    public class ApplyExpenseTypeController : BaseApiController
    {
        private readonly IApplyExpenseTypeService _applyExpenseTypeService;

        public ApplyExpenseTypeController(IApplyExpenseTypeService applyExpenseTypeService)
        {
            _applyExpenseTypeService = applyExpenseTypeService;
        }

        public object Get()
        {
            Mapper.CreateMap<ApplyExpenseType, ApplyExpenseTypeModel>();
            return
                _applyExpenseTypeService.GetApplyExpenseTypes()
                    .Select(Mapper.Map<ApplyExpenseType, ApplyExpenseTypeModel>);
        }
    }
}
