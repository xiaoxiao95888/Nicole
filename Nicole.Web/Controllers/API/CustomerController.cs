using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Nicole.Library.Models;
using Nicole.Library.Models.Enum;
using Nicole.Library.Services;
using Nicole.Web.Infrastructure;
using Nicole.Web.Models;

namespace Nicole.Web.Controllers.API
{
    public class CustomerController : BaseApiController
    {
        private readonly ICustomerService _customerService;
        private readonly IEmployeesService _employeesService;
        private readonly IPositionService _positionService;
        public CustomerController(ICustomerService customerService, IEmployeesService employeesService, IPositionService positionService)
        {
            _customerService = customerService;
            _employeesService = employeesService;
            _positionService = positionService;
        }
        public object Get()
        {
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var currentPosition =
                _employeesService.GetEmployee(HttpContext.Current.User.Identity.GetUser().EmployeeId)
                    .EmployeePostions.Where(
                        n => n.StartDate <= currentDate && (n.EndDate == null || n.EndDate >= currentDate))
                    .Select(n => n.Position)
                    .FirstOrDefault();
            var subpositions = _positionService.GetPositions().Where(n => n.Parent.Id == currentPosition.Id).Select(p=>p.Id).ToArray();

            var pageIndex = string.IsNullOrEmpty(HttpContext.Current.Request["pageIndex"])
                ? 1
                : Convert.ToInt32(HttpContext.Current.Request["pageIndex"]);
            var pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            var codeKey = HttpContext.Current.Request["CodeKey"] ?? string.Empty;
            var nameKey = HttpContext.Current.Request["NameKey"] ?? string.Empty;
            var addressKey = HttpContext.Current.Request["AddressKey"] ?? string.Empty;
            var emailKey = HttpContext.Current.Request["EmailKey"] ?? string.Empty;
            var contactPersonKey = HttpContext.Current.Request["ContactPersonKey"] ?? string.Empty;
            var telNumberKey = HttpContext.Current.Request["TelNumberKey"] ?? string.Empty;
            var customerTypeKey = HttpContext.Current.Request["CustomerTypeKey"] ?? string.Empty;
            var originKey = HttpContext.Current.Request["OriginKey"] ?? string.Empty;
            var result =
                _customerService.GetCustomers()
                    .Where(
                        n => currentPosition.Parent == null || (
                            n.PositionCustomers.Any(
                                p => p.PositionId != null && subpositions.Contains(p.PositionId.Value))));
            if (!string.IsNullOrEmpty(codeKey))
            {
                result = result.Where(n => n.Code.Contains(codeKey));
            }
            if (!string.IsNullOrEmpty(nameKey))
            {
                result = result.Where(n => n.Name.Contains(nameKey));
            }
            if (!string.IsNullOrEmpty(addressKey))
            {
                result = result.Where(n => n.Address.Contains(addressKey));
            }
            if (!string.IsNullOrEmpty(emailKey))
            {
                result = result.Where(n => n.Email.Contains(emailKey));
            }
            if (!string.IsNullOrEmpty(contactPersonKey))
            {
                result = result.Where(n => n.ContactPerson.Contains(contactPersonKey));
            }
            if (!string.IsNullOrEmpty(telNumberKey))
            {
                result = result.Where(n => n.TelNumber.Contains(telNumberKey));
            }
            if (string.IsNullOrEmpty(customerTypeKey) == false && customerTypeKey != "请选择")
            {
                var type = (CustomerType)Enum.Parse(typeof(CustomerType), customerTypeKey, false);
                result = result.Where(n => n.CustomerType == type);

            }
            if (!string.IsNullOrEmpty(originKey))
            {
                result = result.Where(n => n.Origin.Contains(originKey));
            }
            Mapper.Reset();
            Mapper.CreateMap<Employee, EmployeeModel>();
            Mapper.CreateMap<Customer, CustomerModel>()
                .ForMember(n => n.CustomerType, opt => opt.MapFrom(src => src.CustomerType))
                .ForMember(n => n.EmployeeModel,
                    opt =>
                        opt.MapFrom(
                            src =>
                                src.Position.EmployeePostions.Where(
                                    p => p.StartDate <= currentDate && (p.EndDate == null || p.EndDate >= currentDate))
                                    .Select(p => p.Employee)
                                    .FirstOrDefault()))
                .ForMember(n => n.EmployeeModels,
                    opt =>
                        opt.MapFrom(
                            src =>
                                src.PositionCustomers.SelectMany(
                                    p =>
                                        p.Position.EmployeePostions.Where(
                                            ep =>
                                                ep.StartDate <= currentDate &&
                                                (ep.EndDate == null || ep.EndDate >= currentDate))
                                            .Select(rp => rp.Employee))));
            var model = new CustomerModelSettingModel
            {
                Models =
                    result
                        .OrderByDescending(n => n.CreatedTime)
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .Select(Mapper.Map<Customer, CustomerModel>)
                        .ToArray(),
                CurrentPageIndex = pageIndex,
                AllPage = (result.Count() / pageSize) + (result.Count() % pageSize == 0 ? 0 : 1)
            };
            return model;

        }
        public object Get(Guid id)
        {
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            Mapper.Reset();
            Mapper.CreateMap<Employee, EmployeeModel>();
            Mapper.CreateMap<Customer, CustomerModel>()
                .ForMember(n => n.CustomerType, opt => opt.MapFrom(src => src.CustomerType))
                .ForMember(n => n.EmployeeModel, opt => opt.MapFrom(src => src.Position.EmployeePostions.Where(p => p.StartDate <= currentDate && (p.EndDate == null || p.EndDate >= currentDate)).Select(p => p.Employee).FirstOrDefault()));
            return Mapper.Map<Customer, CustomerModel>(_customerService.GetCustomer(id));
        }
    }
}
