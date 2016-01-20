using AutoMapper;
using Nicole.Library.Models;
using Nicole.Library.Services;
using Nicole.Web.Infrastructure;
using Nicole.Web.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Nicole.Web.Controllers.API
{
    public class EnquiryManagerController : BaseApiController
    {
        private readonly IProductService _productService;
        private readonly IEnquiryService _enquiryService;
        private readonly ICustomerService _customerService;
        private readonly IEmployeesService _employeesService;
        public EnquiryManagerController(IProductService productService, IEnquiryService enquiryService, ICustomerService customerService, IEmployeesService employeesService)
        {
            _productService = productService;
            _enquiryService = enquiryService;
            _customerService = customerService;
            _employeesService = employeesService;
        }
        public object Get()
        {
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var currentUser = HttpContext.Current.User.Identity.GetUser();
            var positionId = _employeesService.GetEmployee(currentUser.EmployeeId).EmployeePostions.Where(n => n.StartDate <= currentDate && (n.EndDate == null || n.EndDate >= currentDate)).Select(n => n.Position.Id).FirstOrDefault();
            var pageIndex = string.IsNullOrEmpty(HttpContext.Current.Request["pageIndex"])
                ? 1
                : Convert.ToInt32(HttpContext.Current.Request["pageIndex"]);
            var pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            var customerNameKey = HttpContext.Current.Request["CustomerNameKey"] ?? string.Empty;
            var productTypeKey = HttpContext.Current.Request["ProductTypeKey"] ?? string.Empty;
            var voltageKey = HttpContext.Current.Request["VoltageKey"] ?? string.Empty;
            var capacityKey = HttpContext.Current.Request["CapacityKey"] ?? string.Empty;
            var pitchKey = HttpContext.Current.Request["PitchKey"] ?? string.Empty;
            var levelKey = HttpContext.Current.Request["LevelKey"] ?? string.Empty;
            var partNumberKey = HttpContext.Current.Request["PartNumberKey"] ?? string.Empty;
            var specificDesignKey = HttpContext.Current.Request["SpecificDesignKey"] ?? string.Empty;
            var result = _enquiryService.GetEnquiries().Where(n=>n.PositionId== positionId);
            if (!string.IsNullOrEmpty(customerNameKey))
            {
                result = result.Where(n => n.Customer.Name.Contains(customerNameKey));
            }
            if (!string.IsNullOrEmpty(productTypeKey))
            {
                result = result.Where(n => n.Product.ProductType.Contains(productTypeKey));
            }
            if (!string.IsNullOrEmpty(voltageKey))
            {
                result = result.Where(n => n.Product.Voltage.Contains(voltageKey));
            }
            if (!string.IsNullOrEmpty(capacityKey))
            {
                result = result.Where(n => n.Product.Capacity.Contains(capacityKey));
            }
            if (!string.IsNullOrEmpty(pitchKey))
            {
                result = result.Where(n => n.Product.Pitch.Contains(pitchKey));
            }
            if (!string.IsNullOrEmpty(levelKey))
            {
                result = result.Where(n => n.Product.Level.Contains(levelKey));
            }
            if (!string.IsNullOrEmpty(specificDesignKey))
            {
                result = result.Where(n => n.Product.SpecificDesign.Contains(specificDesignKey));
            }
            if (!string.IsNullOrEmpty(partNumberKey))
            {
                result = result.Where(n => n.Product.PartNumber.Contains(partNumberKey));
            }
            Mapper.Reset();
            Mapper.CreateMap<Product, ProductModel>();
            Mapper.CreateMap<Employee, EmployeeModel>();
            Mapper.CreateMap<Customer, CustomerModel>();
            Mapper.CreateMap<Enquiry, EnquiryModel>()
                .ForMember(n => n.ProductModel, opt => opt.MapFrom(src => src.Product))
                .ForMember(n => n.EmployeeModel, opt => opt.MapFrom(src => src.Position.EmployeePostions.Where(p => p.StartDate <= currentDate && p.EndDate >= currentDate).Select(p => p.Employee).FirstOrDefault()))
                .ForMember(n => n.CustomerModel, opt => opt.MapFrom(src => src.Customer));
            var model = new EnquiryManagerModel
            {
                EnquiryModels =
                    result
                        .OrderByDescending(n => n.CreatedTime)
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .Select(Mapper.Map<Enquiry, EnquiryModel>)
                        .ToArray(),
                CurrentPageIndex = pageIndex,
                AllPage = (result.Count() / pageSize) + (result.Count() % pageSize == 0 ? 0 : 1)
            };
            return model;
        }
        public object Post(EnquiryModel model)
        {
            if (model == null)
            {
                return Failed("信息不完整不能提交");
            }
            if (string.IsNullOrEmpty(model.CustomerModel.Code))
            {
                return Failed("信息不完整不能提交");
            }
            if (string.IsNullOrEmpty(model.ProductModel.PartNumber))
            {
                return Failed("信息不完整不能提交");
            }
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var currentUser = HttpContext.Current.User.Identity.GetUser();
            var product = _productService.GetProducts().FirstOrDefault(n => n.PartNumber == model.ProductModel.PartNumber.Trim());
            var customer = _customerService.GetCustomers().FirstOrDefault(n => n.Code == model.CustomerModel.Code.Trim());
            var position = _employeesService.GetEmployee(currentUser.EmployeeId).EmployeePostions.Where(n => n.StartDate <= currentDate && (n.EndDate == null || n.EndDate >= currentDate)).Select(p => p.Position).FirstOrDefault();
            if (product != null && customer != null && position != null)
            {
                try
                {
                    _enquiryService.Insert(new Enquiry
                    {
                        Id = Guid.NewGuid(),
                        CustomerId = customer.Id,
                        ProductId = product.Id,
                        PositionId = position.Id
                    });
                    //发送邮件
                    return Success();
                }
                catch (Exception ex)
                {
                    return Failed(ex.Message);
                }

            }
            return Failed("信息不匹配不能提交");
        }
    }
}
