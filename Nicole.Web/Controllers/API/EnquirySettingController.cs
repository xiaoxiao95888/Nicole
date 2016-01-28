using AutoMapper;
using Nicole.Library.Models;
using Nicole.Library.Services;
using Nicole.Web.Infrastructure;
using Nicole.Web.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using Nicole.Web.MapperHelper;

namespace Nicole.Web.Controllers.API
{
    public class EnquirySettingController : BaseApiController
    {
        private readonly IProductService _productService;
        private readonly IEnquiryService _enquiryService;
        private readonly ICustomerService _customerService;
        private readonly IEmployeesService _employeesService;
        private readonly IMapperFactory _mapperFactory;
        public EnquirySettingController(IProductService productService, IEnquiryService enquiryService, ICustomerService customerService, IEmployeesService employeesService, IMapperFactory mapperFactory)
        {
            _productService = productService;
            _enquiryService = enquiryService;
            _customerService = customerService;
            _employeesService = employeesService;
            _mapperFactory = mapperFactory;
        }
        public object Get([FromUri] EnquiryModel key, int pageIndex = 1)
        {
            var pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);

            var result = _enquiryService.GetEnquiries();
            if (key.PositionModel != null)
            {
                result = result.Where(n => n.Position.Id == key.PositionModel.Id);
            }
            if (key.CustomerModel != null)
            {
                result = result.Where(n => n.CustomerId == key.CustomerModel.Id);
            }
            if (key.ProductModel != null)
            {
                result = result.Where(n => n.ProductId == key.ProductModel.Id);
            }
            _mapperFactory.GetEnquiryMapper().Create();

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

        //public object Post(EnquiryModel model)
        //{
        //    if (model == null)
        //    {
        //        return Failed("信息不完整不能提交");
        //    }
        //    if (string.IsNullOrEmpty(model.CustomerModel.Code))
        //    {
        //        return Failed("信息不完整不能提交");
        //    }
        //    if (string.IsNullOrEmpty(model.ProductModel.PartNumber))
        //    {
        //        return Failed("信息不完整不能提交");
        //    }
        //    var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //    var currentUser = HttpContext.Current.User.Identity.GetUser();
        //    var product = _productService.GetProducts().FirstOrDefault(n => n.PartNumber == model.ProductModel.PartNumber.Trim());
        //    var customer = _customerService.GetCustomers().FirstOrDefault(n => n.Code == model.CustomerModel.Code.Trim());
        //    var position = _employeesService.GetEmployee(currentUser.EmployeeId).EmployeePostions.Where(n => n.StartDate <= currentDate && (n.EndDate == null || n.EndDate >= currentDate)).Select(p => p.Position).FirstOrDefault();
        //    if (product != null && customer != null && position != null)
        //    {
        //        try
        //        {
        //            _enquiryService.Insert(new Enquiry
        //            {
        //                Id = Guid.NewGuid(),
        //                CustomerId = customer.Id,
        //                ProductId = product.Id,
        //                PositionId = position.Id
        //            });
        //            //发送邮件
        //            return Success();
        //        }
        //        catch (Exception ex)
        //        {
        //            return Failed(ex.Message);
        //        }

        //    }
        //    return Failed("信息不匹配不能提交");
        //}
    }
}
