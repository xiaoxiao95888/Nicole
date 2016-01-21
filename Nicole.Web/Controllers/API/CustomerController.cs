using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using AutoMapper;
using Nicole.Library.Models;
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
        public object Get([FromUri] CustomerModel key, int pageIndex = 1)
        {
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var currentPosition =
                _employeesService.GetEmployee(HttpContext.Current.User.Identity.GetUser().EmployeeId)
                    .EmployeePostions.Where(
                        n => n.StartDate <= currentDate && (n.EndDate == null || n.EndDate >= currentDate))
                    .Select(n => n.Position)
                    .FirstOrDefault();
            var subpositions = _positionService.GetPositions().Where(n => n.Parent.Id == currentPosition.Id).Select(p => p.Id).ToArray();


            var pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);

            var result =
                _customerService.GetCustomers()
                    .Where(
                        n => (
                            n.PositionCustomers.Any(
                                p => p.PositionId != null && (subpositions.Any() ? subpositions.Contains(p.PositionId.Value) : p.PositionId == currentPosition.Id))));
            if (currentPosition.Parent == null)
            {
                result =
                _customerService.GetCustomers();
            }

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
            Mapper.Reset();
            Mapper.CreateMap<Employee, EmployeeModel>();
            Mapper.CreateMap<CustomerType, CustomerTypeModel>();
            Mapper.CreateMap<Customer, CustomerModel>()
                .ForMember(n => n.CustomerTypeModel, opt => opt.MapFrom(src => src.CustomerType))
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
            Mapper.CreateMap<CustomerType, CustomerTypeModel>();
            Mapper.CreateMap<Employee, EmployeeModel>();
            Mapper.CreateMap<Customer, CustomerModel>()
                .ForMember(n => n.CustomerTypeModel, opt => opt.MapFrom(src => src.CustomerType))
                .ForMember(n => n.EmployeeModel, opt => opt.MapFrom(src => src.Position.EmployeePostions.Where(p => p.StartDate <= currentDate && (p.EndDate == null || p.EndDate >= currentDate)).Select(p => p.Employee).FirstOrDefault()));
            return Mapper.Map<Customer, CustomerModel>(_customerService.GetCustomer(id));
        }
    }
}
