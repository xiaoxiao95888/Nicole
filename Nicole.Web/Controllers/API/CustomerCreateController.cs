using AutoMapper;
using Nicole.Library.Models;
using Nicole.Library.Models.Enum;
using Nicole.Library.Services;
using Nicole.Web.Infrastructure;
using Nicole.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Nicole.Web.Controllers.API
{
    public class CustomerCreateController : BaseApiController
    {
        private readonly ICustomerService _customerService;
        private readonly IEmployeesService _employeesService;
        public CustomerCreateController(ICustomerService customerService, IEmployeesService employeesService)
        {
            _customerService = customerService;
            _employeesService = employeesService;
        }
        public object Get()
        {
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
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

            var result = _customerService.GetCustomers();
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

        public object Post(CustomerModel model)
        {
            var errormessage = string.Empty;
            if (model == null)
            {
                errormessage = "客户不得为空";
            }
            else if (string.IsNullOrEmpty(model.Name))
            {
                errormessage = "名称不能为空";
            }
            if (string.IsNullOrEmpty(errormessage))
            {
                if (_customerService.GetCustomers().Any() && _customerService.GetCustomers().Any(n => n.Name == model.Name.Trim()))
                {
                    errormessage = "客户名称重复";
                }
                else
                {
                    var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    var currentUser = HttpContext.Current.User.Identity.GetUser();
                    var positionId = _employeesService.GetEmployee(currentUser.EmployeeId).EmployeePostions.Where(n => n.StartDate <= currentDate && (n.EndDate == null || n.EndDate >= currentDate)).Select(n => n.Position.Id).FirstOrDefault();
                    var item = new Customer
                    {
                        Id = Guid.NewGuid(),
                        PositionId = positionId,
                        Name = model.Name.Trim(),
                        Address = string.IsNullOrEmpty(model.Address) ? null : model.Address.Trim(),
                        Email = string.IsNullOrEmpty(model.Email) ? null : model.Email.Trim(),
                        ContactPerson = string.IsNullOrEmpty(model.ContactPerson) ? null : model.ContactPerson.Trim(),
                        TelNumber = string.IsNullOrEmpty(model.TelNumber) ? null : model.TelNumber.Trim(),
                        Origin = string.IsNullOrEmpty(model.Origin) ? null : model.Origin.Trim(),
                        CustomerType = (CustomerType?)model.CustomerType
                    };
                    try
                    {
                        _customerService.Insert(item);
                    }
                    catch (Exception ex)
                    {
                        return Failed(ex.Message);
                    }
                }
            }
            return string.IsNullOrEmpty(errormessage) ? Success() : Failed(errormessage);
        }
        public object Put(CustomerModel model)
        {
            var errormessage = string.Empty;
            if (model == null)
            {
                errormessage = "客户不得为空";
            }
            else if (string.IsNullOrEmpty(model.Name))
            {
                errormessage = "名称不能为空";
            }
            else if (_customerService.GetCustomers().Any() && _customerService.GetCustomers().Any(n => n.Name == model.Name.Trim() && n.Id != model.Id))
            {
                errormessage = "客户名称重复";
            }
            else
            {
                var item = _customerService.GetCustomer(model.Id);
                if (item.IsDeleted)
                {
                    errormessage = "该客户已删除";
                }
                if (string.IsNullOrEmpty(errormessage))
                {
                    var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    var currentUser = HttpContext.Current.User.Identity.GetUser();
                    var positionId = _employeesService.GetEmployee(currentUser.EmployeeId).EmployeePostions.Where(n => n.StartDate <= currentDate && (n.EndDate == null || n.EndDate >= currentDate)).Select(n => n.Position.Id).FirstOrDefault();
                    item.PositionId = positionId;
                    item.Name = model.Name.Trim();
                    item.Address = string.IsNullOrEmpty(model.Address) ? null : model.Address.Trim();
                    item.Email = string.IsNullOrEmpty(model.Email) ? null : model.Email.Trim();
                    item.ContactPerson = string.IsNullOrEmpty(model.ContactPerson) ? null : model.ContactPerson.Trim();
                    item.TelNumber = string.IsNullOrEmpty(model.TelNumber) ? null : model.TelNumber.Trim();
                    item.Origin = string.IsNullOrEmpty(model.Origin) ? null : model.Origin.Trim();
                    item.CustomerType = (CustomerType?)model.CustomerType;
                    try
                    {
                        _customerService.Update();
                    }
                    catch (Exception ex)
                    {
                        errormessage = ex.Message;
                    }
                }
            }
            return string.IsNullOrEmpty(errormessage) ? Success() : Failed(errormessage);

        }
        public object Delete(Guid id)
        {
            try
            {
                var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                var currentUser = HttpContext.Current.User.Identity.GetUser();
                var positionId = _employeesService.GetEmployee(currentUser.EmployeeId).EmployeePostions.Where(n => n.StartDate <= currentDate && (n.EndDate == null || n.EndDate >= currentDate)).Select(n => n.Position.Id).FirstOrDefault();
                var item = _customerService.GetCustomer(id);
                if (item.PositionCustomers.Any())
                {
                    return Failed("该客户已分配给销售，禁止删除");
                }
                item.IsDeleted = true;
                item.PositionId = positionId;
                _customerService.Update();
            }
            catch (Exception ex)
            {
                return Failed(ex.Message);
            }
            return Success();
        }
    }

}
