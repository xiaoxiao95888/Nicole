using System;
using System.Collections.Generic;
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
    public class OrderDetailController : BaseApiController
    {
        private readonly IAuditLevelService _auditLevelService;
        private readonly IEnquiryService _enquiryService;
        private readonly ICustomerService _customerService;
        private readonly IEmployeesService _employeesService;
        private readonly IMapperFactory _mapperFactory;
        private readonly IPositionService _positionService;
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IPayPeriodService _payPeriodService;

        public OrderDetailController(IAuditLevelService auditLevelService, IEnquiryService enquiryService,
            IMapperFactory mapperFactory, IEmployeesService employeesService, IPositionService positionService,
            IOrderService orderService, IPayPeriodService payPeriodService, ICustomerService customerService, IOrderDetailService orderDetailService)
        {
            _auditLevelService = auditLevelService;
            _enquiryService = enquiryService;
            _mapperFactory = mapperFactory;
            _employeesService = employeesService;
            _positionService = positionService;
            _orderService = orderService;
            _payPeriodService = payPeriodService;
            _customerService = customerService;
            _orderDetailService = orderDetailService;
        }

        public object GetByOrderId(Guid id)
        {
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var currentPosition =
                _employeesService.GetEmployee(HttpContext.Current.User.Identity.GetUser().EmployeeId)
                    .EmployeePostions.Where(
                        n => n.StartDate <= currentDate && (n.EndDate == null || n.EndDate >= currentDate))
                    .Select(n => n.Position)
                    .FirstOrDefault();
            var subpositions =
                _positionService.GetPositions()
                    .Where(n => n.Parent.Id == currentPosition.Id || n.Id == currentPosition.Id)
                    .Select(p => p.Id)
                    .ToArray();
            _mapperFactory.GetOrderMapper().OrderDetail();
            return
                _orderDetailService.GetOrderDetails(id)
                    .Where(n => subpositions.Contains(n.Order.PositionId.Value))
                    .Select(Mapper.Map<OrderDetail, OrderDetailModel>);
        }
    }
}
