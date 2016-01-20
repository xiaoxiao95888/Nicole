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
    public class LeftNavigationController : BaseApiController
    {
        private readonly ILeftNavigationsService _leftNavigationsService;

        public LeftNavigationController(ILeftNavigationsService leftNavigationsService)
        {
            _leftNavigationsService = leftNavigationsService;
        }

        public object Get()
        {
            Mapper.Reset();
            Mapper.CreateMap<LeftNavigation, LeftNavigationModel>();
            return _leftNavigationsService.GetLeftNavigations().Select(Mapper.Map<LeftNavigation, LeftNavigationModel>);
        }
    }
}
