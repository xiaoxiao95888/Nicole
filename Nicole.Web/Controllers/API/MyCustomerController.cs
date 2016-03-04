using Nicole.Library.Services;
using Nicole.Web.MapperHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AutoMapper;
using Nicole.Library.Models;
using Nicole.Web.Infrastructure;
using Nicole.Web.Models;

namespace Nicole.Web.Controllers.API
{
    public class MyCustomerController : BaseApiController
    {
        private readonly ICustomerService _customerService;
        private readonly IEmployeesService _employeesService;
        private readonly IPositionService _positionService;
        private readonly IMapperFactory _mapperFactory;
        public MyCustomerController(ICustomerService customerService, IEmployeesService employeesService, IPositionService positionService, IMapperFactory mapperFactory)
        {
            _customerService = customerService;
            _employeesService = employeesService;
            _positionService = positionService;
            _mapperFactory = mapperFactory;
        }
        public object Get([FromUri] CustomerModel key, int pageIndex = 1)
        {
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var currentPosition =
                _employeesService.GetEmployee(HttpContext.Current.User.Identity.GetUser().EmployeeId)
                    .EmployeePostions.Where(
                        n => n.StartDate <= currentDate && (n.EndDate == null || n.EndDate >= currentDate) && n.IsDeleted == false)
                    .Select(n => n.Position)
                    .FirstOrDefault();
            var subpositions = _positionService.GetPositions().Where(n => n.Parent.Id == currentPosition.Id || n.Id== currentPosition.Id).Select(p => p.Id).ToArray();

            var pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);

            var result =
                _customerService.GetCustomers()
                    .Where(
                        n => (
                            n.PositionCustomers.Any(
                                p => p.PositionId != null && (subpositions.Any() ? subpositions.Contains(p.PositionId.Value) : p.PositionId == currentPosition.Id))));
           

            result = result.Where(n => (key.Name == null || n.Name.Contains(key.Name))
                                       && (key.Origin == null || n.Origin.Contains(key.Origin))
                                       && (key.TelNumber == null || n.TelNumber.Contains(key.TelNumber))
                                       && (key.Address == null || n.Address.Contains(key.Address))
                                       && (key.Code == null || n.Code.Contains(key.Code))
                                       && (key.ContactPerson == null || n.ContactPerson.Contains(key.ContactPerson))
                                       && (key.Email == null || n.Email.Contains(key.Email)));

            if (key.CustomerTypeModel != null && key.CustomerTypeModel.Id!=Guid.Empty)
            {
                result = result.Where(n => n.CustomerType != null && n.CustomerTypeId == key.CustomerTypeModel.Id);
            }
            _mapperFactory.GetCustomerMapper().Create();
            var model = new CustomerModelSettingModel
            {
                Models =
                    result
                        .OrderByDescending(n => n.UpdateTime)
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .Select(Mapper.Map<Customer, CustomerModel>)
                        .ToArray(),
                CurrentPageIndex = pageIndex,
                AllPage = (result.Count() / pageSize) + (result.Count() % pageSize == 0 ? 0 : 1)
            };
            return model;

        }
    }
}
