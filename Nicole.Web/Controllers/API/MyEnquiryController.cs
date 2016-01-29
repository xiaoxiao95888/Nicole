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
using Nicole.Library.Services;
using Nicole.Web.Infrastructure;
using Nicole.Web.MapperHelper;
using Nicole.Web.Models;

namespace Nicole.Web.Controllers.API
{
    public class MyEnquiryController : BaseApiController
    {
        private readonly IEnquiryService _enquiryService;
        private readonly IMapperFactory _mapperFactory;
        private readonly IEmployeesService _employeesService;
        private readonly IPositionService _positionService;
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;
        public MyEnquiryController(IEnquiryService enquiryService,
            IMapperFactory mapperFactory,
            IEmployeesService employeesService,
            IPositionService positionService,
            IProductService productService,
            ICustomerService customerService)
        {
            _enquiryService = enquiryService;
            _mapperFactory = mapperFactory;
            _employeesService = employeesService;
            _positionService = positionService;
            _productService = productService;
            _customerService = customerService;
        }
        public object Get([FromUri]EnquiryModel key, int pageIndex=1)
        {
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var currentPosition =
                _employeesService.GetEmployee(HttpContext.Current.User.Identity.GetUser().EmployeeId)
                    .EmployeePostions.Where(
                        n => n.StartDate <= currentDate && (n.EndDate == null || n.EndDate >= currentDate))
                    .Select(n => n.Position)
                    .FirstOrDefault();
            var subpositions = _positionService.GetPositions().Where(n => n.Parent.Id == currentPosition.Id || n.Id == currentPosition.Id).Select(p => p.Id).ToArray();
            var pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            var result =
                _enquiryService.GetEnquiries()
                    .Where(n => subpositions.Contains(n.PositionId.Value));
            if (key.CustomerModel != null)
            {
                result =
                    result.Where(
                        n => (key.CustomerModel.Code == null || n.Customer.Code.Contains(key.CustomerModel.Code.Trim()))
                             &&
                             (key.CustomerModel.Name == null || n.Customer.Name.Contains(key.CustomerModel.Name.Trim()))
                             &&
                             (key.CustomerModel.Address == null || n.Customer.Address.Contains(key.CustomerModel.Address.Trim()))
                             &&
                             (key.CustomerModel.ContactPerson == null || n.Customer.ContactPerson.Contains(key.CustomerModel.ContactPerson.Trim()))
                             &&
                             (key.CustomerModel.Email == null || n.Customer.Email.Contains(key.CustomerModel.Email.Trim()))
                             &&
                             (key.CustomerModel.Origin == null || n.Customer.Origin.Contains(key.CustomerModel.Origin.Trim()))
                              &&
                             (key.CustomerModel.TelNumber == null || n.Customer.TelNumber.Contains(key.CustomerModel.TelNumber.Trim()))
                        );
            }
            if (key.ProductModel != null)
            {
                result =
                    result.Where(
                        n => (key.ProductModel.PartNumber == null || n.Product.PartNumber.Contains(key.ProductModel.PartNumber.Trim()))
                             &&
                             (key.ProductModel.Capacity == null || n.Product.Capacity.Contains(key.ProductModel.Capacity.Trim()))
                             &&
                             (key.ProductModel.Level == null || n.Product.Level.Contains(key.ProductModel.Level.Trim()))
                             &&
                             (key.ProductModel.Pitch == null || n.Product.Pitch.Contains(key.ProductModel.Pitch.Trim()))
                             &&
                             (key.ProductModel.ProductType == null || n.Product.ProductType.Contains(key.ProductModel.ProductType.Trim()))
                             &&
                             (key.ProductModel.SpecificDesign == null || n.Product.SpecificDesign.Contains(key.ProductModel.SpecificDesign.Trim()))
                              &&
                             (key.ProductModel.Voltage == null || n.Product.Voltage.Contains(key.ProductModel.Voltage.Trim()))
                        );
            }
            _mapperFactory.GetEnquiryMapper().Create();
            var model = new EnquiryManagerModel
            {
                EnquiryModels =
                    result
                        .OrderByDescending(n => n.UpdateTime)
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .Select(Mapper.Map<Enquiry, EnquiryModel>)
                        .ToArray(),
                CurrentPageIndex = pageIndex,
                AllPage = (result.Count() / pageSize) + (result.Count() % pageSize == 0 ? 0 : 1)
            };
            return model;
        }

        public object Get(Guid id)
        {
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var currentPosition =
                _employeesService.GetEmployee(HttpContext.Current.User.Identity.GetUser().EmployeeId)
                    .EmployeePostions.Where(
                        n => n.StartDate <= currentDate && (n.EndDate == null || n.EndDate >= currentDate))
                    .Select(n => n.Position)
                    .FirstOrDefault();
            var subpositions = _positionService.GetPositions().Where(n => n.Parent.Id == currentPosition.Id || n.Id == currentPosition.Id).Select(p => p.Id).ToArray();
            var result =
                _enquiryService
                    .GetEnquiries().FirstOrDefault(n => subpositions.Contains(n.PositionId.Value) && n.Id==id);
           
            _mapperFactory.GetEnquiryMapper().Create();

            return Mapper.Map<Enquiry, EnquiryModel>(result);
        }
        public object Post(EnquiryModel model)
        {
            if (model == null)
            {
                return Failed("询价不得为空");
            }
            if (model.CustomerModel == null || model.ProductModel == null)
            {
                return Failed("产品或者客户不得为空");
            }
            if (string.IsNullOrEmpty(model.ProductModel.PartNumber))
            {
                return Failed("料号不得为空");
            }
            var customer = _customerService.GetCustomer(model.CustomerModel.Id);
            if (customer == null)
            {
                return Failed("找不到客户");
            }
            var product =
               _productService.GetProducts().FirstOrDefault(n => n.PartNumber == model.ProductModel.PartNumber.Trim());
            if (product == null)
            {
                if (_productService.GetProducts().Any(n => n.PartNumber == model.ProductModel.PartNumber.Trim()))
                {

                    return Failed("料号已存在");
                }
                product = new Product
                {
                    Id = Guid.NewGuid(),
                    PartNumber = model.ProductModel.PartNumber.Trim(),
                    ProductType = string.IsNullOrEmpty(model.ProductModel.ProductType)
                            ? model.ProductModel.ProductType
                            : model.ProductModel.ProductType.Trim().ToUpper(),
                    Voltage =
                        string.IsNullOrEmpty(model.ProductModel.Voltage)
                            ? model.ProductModel.Voltage
                            : model.ProductModel.Voltage.Trim(),
                    Capacity =
                        string.IsNullOrEmpty(model.ProductModel.Capacity)
                            ? model.ProductModel.Capacity
                            : model.ProductModel.Capacity.Trim(),
                    Pitch =
                        string.IsNullOrEmpty(model.ProductModel.Pitch)
                            ? model.ProductModel.Pitch
                            : model.ProductModel.Pitch.Trim(),
                    Level =
                        string.IsNullOrEmpty(model.ProductModel.Level)
                            ? model.ProductModel.Level
                            : model.ProductModel.Level.Trim(),
                    SpecificDesign =
                        string.IsNullOrEmpty(model.ProductModel.SpecificDesign)
                            ? model.ProductModel.SpecificDesign
                            : model.ProductModel.SpecificDesign.Trim(),

                };
                _productService.Insert(product);
            }
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var currentPosition =
                _employeesService.GetEmployee(HttpContext.Current.User.Identity.GetUser().EmployeeId)
                    .EmployeePostions.Where(
                        n => n.StartDate <= currentDate && (n.EndDate == null || n.EndDate >= currentDate))
                    .Select(n => n.Position)
                    .FirstOrDefault();
            if (currentPosition == null)
            {
                return Failed("找不到相关职位");
            }
            try
            {
                _enquiryService.Insert(new Enquiry
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customer.Id,
                    ProductId = product.Id,
                    PositionId = currentPosition.Id
                });
                return Success();
            }
            catch (Exception ex)
            {
                return Failed(ex.Message);
            }
        }
    }
}
