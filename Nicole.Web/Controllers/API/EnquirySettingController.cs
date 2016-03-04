using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Nicole.Library.Models;
using Nicole.Library.Services;
using Nicole.Web.MapperHelper;
using Nicole.Web.Models;

namespace Nicole.Web.Controllers.API
{
    public class EnquirySettingController : BaseApiController
    {
        private readonly IEnquiryService _enquiryService;
        private readonly IMapperFactory _mapperFactory;
        public EnquirySettingController(IEnquiryService enquiryService, IMapperFactory mapperFactory)
        {
            _enquiryService = enquiryService;
            _mapperFactory = mapperFactory;
        }
        public object Get([FromUri]EnquiryModel key, int pageIndex = 1)
        {
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            var result = _enquiryService.GetEnquiries();
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
            if (key.PositionModel != null && key.PositionModel.CurrentEmployeeModel != null)
            {
                result =
                    result.Where(
                        n =>
                            (key.PositionModel.CurrentEmployeeModel.Name == null ||
                             n.Position.EmployeePostions.Any(
                                 p =>
                                     p.Employee.Name.Contains(key.PositionModel.CurrentEmployeeModel.Name.Trim()) &&
                                     p.StartDate <= currentDate && (p.EndDate == null || p.EndDate >= currentDate) && n.IsDeleted == false))
                            &&
                            (key.PositionModel.CurrentEmployeeModel.Mail == null ||
                             n.Position.EmployeePostions.Any(
                                 p =>
                                     p.Employee.Mail.Contains(key.PositionModel.CurrentEmployeeModel.Mail.Trim()) &&
                                     p.StartDate <= currentDate && (p.EndDate == null || p.EndDate >= currentDate) && n.IsDeleted == false))
                            &&
                            (key.PositionModel.CurrentEmployeeModel.PhoneNumber == null ||
                             n.Position.EmployeePostions.Any(
                                 p =>
                                     p.Employee.PhoneNumber.Contains(
                                         key.PositionModel.CurrentEmployeeModel.PhoneNumber.Trim()) &&
                                     p.StartDate <= currentDate && (p.EndDate == null || p.EndDate >= currentDate) && n.IsDeleted == false))

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

        public object Put(EnquiryModel model)
        {
            if (model == null)
            {
                return Failed("询价为空");
            }
            var item = _enquiryService.GetEnquiry(model.Id);
            if (item == null || item.IsDeleted)
            {
                return Failed("找不到数据");
            }
            item.Price = model.Price;
            try
            {
                _enquiryService.Update();
                //todo 发送邮件
                return Success();
            }
            catch (Exception ex)
            {
                return Failed(ex.Message);
            }
        }
    }
}
