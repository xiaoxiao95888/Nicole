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
    public class PositionController  : BaseApiController
    {
        private readonly IPositionService _positionService;

        public PositionController(IPositionService positionService)
        {
            _positionService = positionService;
        }

        public object Get()
        {
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            Mapper.Reset();
            Mapper.CreateMap<Employee, EmployeeModel>();
            Mapper.CreateMap<Position, PositionModel>()
                .ForMember(n => n.CurrentEmployeeModel,
                    opt =>
                        opt.MapFrom(
                            src =>
                                src.EmployeePostions.Where(
                                    p => p.StartDate <= currentDate && (p.EndDate >= currentDate || p.EndDate == null))
                                    .Select(p => p.Employee)
                                    .FirstOrDefault()));
            return _positionService.GetPositions().Select(Mapper.Map<Position, PositionModel>);
        }
    }
}
