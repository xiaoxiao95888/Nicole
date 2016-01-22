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
    public class MyCustomersController : BaseApiController
    {
        private readonly IEmployeesService _employeesService;
        private readonly IMapperFactory _mapperFactory;
        public MyCustomersController(IEmployeesService employeesService, IMapperFactory mapperFactory)
        {
            _employeesService = employeesService;
            _mapperFactory = mapperFactory;
        }
        public object Get([FromUri] CustomerModel key, int pageIndex = 1)
        {
            _mapperFactory.GetCustomerMapper().Create();
            var pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var currentPosition =
                _employeesService.GetEmployee(HttpContext.Current.User.Identity.GetUser().EmployeeId)
                    .EmployeePostions.Where(
                        n => n.StartDate <= currentDate && (n.EndDate == null || n.EndDate >= currentDate))
                    .Select(n => n.Position)
                    .FirstOrDefault();
            if (currentPosition != null)
            {
                var result =
                    currentPosition.PositionCustomers.Where(n => n.IsDeleted == false
                                                                 && n.Customer.IsDeleted == false
                                                                 &&
                                                                 (key.Name == null || n.Customer.Name.Contains(key.Name))
                                                                 &&
                                                                 (key.Origin == null ||
                                                                  n.Customer.Origin.Contains(key.Origin))
                                                                 &&
                                                                 (key.TelNumber == null ||
                                                                  n.Customer.TelNumber.Contains(key.TelNumber))
                                                                 &&
                                                                 (key.Address == null ||
                                                                  n.Customer.Address.Contains(key.Address))
                                                                 &&
                                                                 (key.Code == null || n.Customer.Code.Contains(key.Code))
                                                                 &&
                                                                 (key.ContactPerson == null ||
                                                                  n.Customer.ContactPerson.Contains(key.ContactPerson))
                                                                 &&
                                                                 (key.Email == null ||
                                                                  n.Customer.Email.Contains(key.Email)));

                if (key.CustomerTypeModel != null)
                {
                    result = result.Where(n => n.Customer.CustomerType != null && n.Customer.CustomerTypeId == key.CustomerTypeModel.Id);
                }
                var model = new CustomerModelSettingModel
                {
                    Models =
                        result
                            .OrderByDescending(n => n.CreatedTime)
                            .Skip((pageIndex - 1) * pageSize)
                            .Take(pageSize)
                            .Select(n => Mapper.Map<Customer, CustomerModel>(n.Customer))
                            .ToArray(),
                    CurrentPageIndex = pageIndex,
                    AllPage = (result.Count() / pageSize) + (result.Count() % pageSize == 0 ? 0 : 1)
                };
                return model;
            }
            return null;

        }
    }
}
