using Nicole.Library.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Nicole.Library.Models;
using Nicole.Web.Infrastructure;
using Nicole.Web.MapperHelper;
using Nicole.Web.Models;

namespace Nicole.Web.Controllers.API
{
    public class OrderController : BaseApiController
    {
        private readonly ICustomerService _customerService;
        private readonly IEnquiryService _enquiryService;
        private readonly IEmployeesService _employeesService;
        private readonly IMapperFactory _mapperFactory;
        private readonly IPositionService _positionService;
        private readonly IOrderService _orderService;
        public OrderController(ICustomerService customerService, IEnquiryService enquiryService, IMapperFactory mapperFactory, IEmployeesService employeesService,IPositionService positionService, IOrderService orderService)
        {
            _customerService = customerService;
            _enquiryService = enquiryService;
            _mapperFactory = mapperFactory;
            _employeesService = employeesService;
            _positionService = positionService;
            _orderService = orderService;
        }

        public object Get(OrderModel key, int pageIndex = 1)
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
                _orderService.GetOrders()
                    .Where(n => subpositions.Contains(n.Enquiry.PositionId.Value));
            if (key.EnquiryModel != null)
            {
                result = result.Where(n => n.EnquiryId == key.EnquiryModel.Id);
            }
            _mapperFactory.GetOrderMapper().Create();
            var model = new OrderManagerModel
            {
                OrderModels = 
                   result
                       .OrderByDescending(n => n.UpdateTime)
                       .Skip((pageIndex - 1) * pageSize)
                       .Take(pageSize)
                       .Select(Mapper.Map<Order, OrderModel>)
                       .ToArray(),
                CurrentPageIndex = pageIndex,
                AllPage = (result.Count() / pageSize) + (result.Count() % pageSize == 0 ? 0 : 1)
            };
            return model;
        }

        public object Get(Guid id)
        {
            return null;
        }
    }
}