using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using AutoMapper;
using Nicole.Library.Models;
using Nicole.Library.Services;
using Nicole.Web.Infrastructure;
using Nicole.Web.MapperHelper;
using Nicole.Web.Models;

namespace Nicole.Web.Controllers.API
{
    public class CustomerController : BaseApiController
    {
        private readonly ICustomerService _customerService;
        private readonly IEmployeesService _employeesService;
        private readonly IPositionService _positionService;
        private readonly IMapperFactory _mapperFactory;
        public CustomerController(ICustomerService customerService, IEmployeesService employeesService, IPositionService positionService, IMapperFactory mapperFactory)
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
                        n => n.StartDate <= currentDate && (n.EndDate == null || n.EndDate >= currentDate))
                    .Select(n => n.Position)
                    .FirstOrDefault();
           
            var pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);

            var result =
                _customerService.GetCustomers();
            result = result.Where(n => (key.Name == null || n.Name.Contains(key.Name))
                                       && (key.Origin == null || n.Origin.Contains(key.Origin))
                                       && (key.TelNumber == null || n.TelNumber.Contains(key.TelNumber))
                                       && (key.Address == null || n.Address.Contains(key.Address))
                                       && (key.Code == null || n.Code.Contains(key.Code))
                                       && (key.ContactPerson == null || n.ContactPerson.Contains(key.ContactPerson))
                                       && (key.Email == null || n.Email.Contains(key.Email)));

            if (key.CustomerTypeModel != null)
            {
                result = result.Where(n => n.CustomerType != null && n.CustomerTypeId == key.CustomerTypeModel.Id);
            }
            _mapperFactory.GetCustomerMapper().Create();
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
                        CustomerTypeId = model.CustomerTypeModel == null ? (Guid?)null : model.CustomerTypeModel.Id
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
                    item.CustomerTypeId = model.CustomerTypeModel != null ? model.CustomerTypeModel.Id : (Guid?)null;
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
