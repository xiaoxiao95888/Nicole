using Nicole.Library.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
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
        private readonly IAuditLevelService _auditLevelService;
        private readonly IEnquiryService _enquiryService;
        private readonly ICustomerService _customerService;
        private readonly IEmployeesService _employeesService;
        private readonly IMapperFactory _mapperFactory;
        private readonly IPositionService _positionService;
        private readonly IOrderService _orderService;
        private readonly IPayPeriodService _payPeriodService;

        public OrderController(IAuditLevelService auditLevelService, IEnquiryService enquiryService,
            IMapperFactory mapperFactory, IEmployeesService employeesService, IPositionService positionService,
            IOrderService orderService, IPayPeriodService payPeriodService, ICustomerService customerService)
        {
            _auditLevelService = auditLevelService;
            _enquiryService = enquiryService;
            _mapperFactory = mapperFactory;
            _employeesService = employeesService;
            _positionService = positionService;
            _orderService = orderService;
            _payPeriodService = payPeriodService;
            _customerService = customerService;
        }

        public object Post(OrderModel model)
        {
            if (model == null || model.OrderDetailModels.Any() == false)
            {
                return Failed("合同不能为空");
            }
            if (model.OrderDetailModels.Any(n => n.Qty == 0))
            {
                return Failed("合同的每个料号都需要数量");
            }
            if (model.OrderDate == DateTime.MinValue)
            {
                return Failed("合同不能没有日期");
            }
            if (model.PayPeriodModel == null)
            {
                return Failed("请选择账期");
            }
            var payPeriod = _payPeriodService.GetPayPeriod(model.PayPeriodModel.Id);
            if (payPeriod == null)
            {
                return Failed("请选择账期");
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
                return Failed("没有权限");
            }
            var orderdetails = new List<OrderDetail>();
            foreach (var item in model.OrderDetailModels)
            {
                var enquiry = _enquiryService.GetEnquiry(item.EnquiryModel.Id);
                if (item.UnitPrice == null)
                {
                    return Failed("请填写单价");
                }
                if (item.Qty == null)
                {
                    return Failed("请填写数量");
                }
                if (enquiry == null || enquiry.IsDeleted || enquiry.PositionId != currentPosition.Id)
                {
                    return Failed("禁止提交");
                }
                if (enquiry.Price == null || item.UnitPrice < enquiry.Price ||
                    (Math.Abs(item.UnitPrice.Value) > Math.Abs(enquiry.Price.Value) * (decimal)1.05))
                {
                    return Failed("单价超出范围");
                }
                orderdetails.Add(new OrderDetail
                {
                    Id = Guid.NewGuid(),
                    Qty = item.Qty.Value,
                    EnquiryId = enquiry.Id,
                    TotalPrice = (decimal)(item.Qty * item.UnitPrice.Value),
                    UnitPrice = item.UnitPrice.Value
                });
            }
            var parentRole =
                _auditLevelService.GetAuditLevels()
                    .Where(n => n.RoleId == currentPosition.Role.Id)
                    .Select(n => n.ParentRole).FirstOrDefault();
            var customer = _customerService.GetCustomer(model.CustomerModel.Id);
            if (parentRole == null)
            {
                return Failed("找不到上级审核人");
            }
            if (customer == null)
            {
                return Failed("找不到客户");
            }
            try
            {
                var item = new Order
                {
                    Id = Guid.NewGuid(),
                    Remark = model.Remark,
                    OrderDate = model.OrderDate,
                    PayPeriodId = model.PayPeriodModel.Id,
                    EstimatedDeliveryDate = model.EstimatedDeliveryDate,
                    LastPayDate = model.OrderDate.AddDays(payPeriod.Days),
                    OrderReviews = new Collection<OrderReview>
                    {
                        new OrderReview
                        {
                            Id = Guid.NewGuid(),
                            SendToRoleId = parentRole.Id
                        }
                    },
                    ContractAmount = orderdetails.Sum(p => p.TotalPrice),
                    OrderDetails = orderdetails,
                    PositionId = currentPosition.Id,
                    CustomerId = customer.Id
                };
                _orderService.Insert(item);
                return Success();
            }
            catch (Exception ex)
            {

                return Failed(ex.Message);
            }


        }

        public object Get([FromUri] OrderModel key, int pageIndex = 1)
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
            var pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            var result =
                _orderService.GetOrders()
                    .Where(n => subpositions.Contains(n.PositionId.Value));

            if (key.CustomerModel != null)
            {
                result =
                    result.Where(
                        n =>
                            (
                                key.CustomerModel.Code == null ||
                                n.Customer.Code.Contains(key.CustomerModel.Code.Trim()))
                            && (
                                key.CustomerModel.Name == null ||
                                n.Customer.Name.Contains(key.CustomerModel.Name.Trim())));
            }

            result = result.Where(n => key.Code == null || n.Code.Contains(key.Code.Trim()));
            _mapperFactory.GetOrderMapper().Create();
            var model = new OrderManagerModel
            {
                Models =
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
            var result =
                _orderService
                    .GetOrders().FirstOrDefault(n => subpositions.Contains(n.PositionId.Value) && n.Id == id);

            _mapperFactory.GetOrderMapper().Create();
            return Mapper.Map<Order, OrderModel>(result);
        }

        public object Put(Guid id, OrderModel model)
        {
            return null;
            //var item = _orderService.GetOrder(id);
            //var enquiry = _enquiryService.GetEnquiry(model.EnquiryModel.Id);
            //if (item == null)
            //{
            //    return Failed("找不到合同");
            //}
            //if (model.OrderDate == DateTime.MinValue)
            //{
            //    return Failed("不能没有合同日期");
            //}
            //if (model.Qty == 0)
            //{
            //    return Failed("合同不能没有数量");
            //}
            //if (model.PayPeriodModel == null)
            //{
            //    return Failed("请选择账期");
            //}
            //var payPeriod = _payPeriodService.GetPayPeriod(model.PayPeriodModel.Id);
            //if (payPeriod == null)
            //{
            //    return Failed("请选择账期");
            //}
            //var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            //var currentPosition =
            //    _employeesService.GetEmployee(HttpContext.Current.User.Identity.GetUser().EmployeeId)
            //        .EmployeePostions.Where(
            //            n => n.StartDate <= currentDate && (n.EndDate == null || n.EndDate >= currentDate))
            //        .Select(n => n.Position)
            //        .FirstOrDefault();
            //if (currentPosition == null)
            //{
            //    return Failed("没有权限");
            //}
            //if (enquiry == null || enquiry.IsDeleted || enquiry.PositionId != currentPosition.Id || enquiry.Price == null)
            //{
            //    return Failed("禁止提交");
            //}

            //if (model.UnitPrice < enquiry.Price || (Math.Abs(model.UnitPrice) > Math.Abs(enquiry.Price.Value) * (decimal)1.05))
            //{
            //    return Failed("单价超出范围");
            //}
            //var parentRole =
            //    _auditLevelService.GetAuditLevels()
            //        .Where(n => n.RoleId == currentPosition.Role.Id)
            //        .Select(n => n.ParentRole).FirstOrDefault();
            //if (parentRole == null)
            //{
            //    return Failed("找不到上级审核人");
            //}
            //item.Qty = model.Qty;
            //item.UnitPrice = model.UnitPrice;
            //item.Remark = model.Remark;
            //item.TotalPrice = model.Qty * model.UnitPrice;
            //item.OrderDate = model.OrderDate;
            //item.EstimatedDeliveryDate = model.EstimatedDeliveryDate;
            //item.PayPeriodId = model.PayPeriodModel.Id;
            //item.LastPayDate = model.OrderDate.AddDays(payPeriod.Days);
            //item.OrderReviews.Add(new OrderReview
            //{
            //    Id = Guid.NewGuid(),
            //    SendToRoleId = parentRole.Id
            //});
            //try
            //{
            //    _orderService.Update();
            //    return Success();
            //}
            //catch (Exception ex)
            //{
            //    return Failed(ex.Message);
            //}

        }

        public object Delete(Guid id)
        {
            var item = _orderService.GetOrder(id);
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var currentPosition =
                _employeesService.GetEmployee(HttpContext.Current.User.Identity.GetUser().EmployeeId)
                    .EmployeePostions.Where(
                        n => n.StartDate <= currentDate && (n.EndDate == null || n.EndDate >= currentDate))
                    .Select(n => n.Position)
                    .FirstOrDefault();
            if (item == null)
            {
                return Failed("找不到合同");
            }
            if (currentPosition == null || item.PositionId != currentPosition.Id)
            {
                return Failed("没有权限");
            }
            if (item.IsApproved)
            {
                return Failed("禁止删除");
            }
            var review = item.OrderReviews.OrderByDescending(n => n.UpdateTime).FirstOrDefault();
            if (review != null)
            {
                if (!review.IsReturn)
                {
                    return Failed("禁止删除");
                }
            }
            item.IsDeleted = true;
            try
            {
                _orderService.Update();
                return Success();
            }
            catch (Exception ex)
            {
                return Failed(ex.Message);
            }
        }
    }

    public class OrderCheckUnitPriceController : BaseApiController
    {
        private readonly IEnquiryService _enquiryService;
        private readonly IEmployeesService _employeesService;


        public OrderCheckUnitPriceController(IEnquiryService enquiryService,
             IEmployeesService employeesService)
        {
            _enquiryService = enquiryService;
            _employeesService = employeesService;
        }

        public object Post(OrderDetailModel model)
        {
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var currentPosition =
                _employeesService.GetEmployee(HttpContext.Current.User.Identity.GetUser().EmployeeId)
                    .EmployeePostions.Where(
                        n => n.StartDate <= currentDate && (n.EndDate == null || n.EndDate >= currentDate))
                    .Select(n => n.Position)
                    .FirstOrDefault();
            var enquiry = _enquiryService.GetEnquiry(model.EnquiryModel.Id);
            if (currentPosition != null &&
                (enquiry == null || enquiry.IsDeleted || enquiry.PositionId != currentPosition.Id))
            {
                return Failed("禁止提交");
            }
            if (model.UnitPrice == null)
            {
                return Failed("请填写单价");
            }
            if (model.Qty == null)
            {
                return Failed("请填写数量");
            }
            if (enquiry.Price == null || model.UnitPrice < enquiry.Price ||
                (Math.Abs(model.UnitPrice.Value) > Math.Abs(enquiry.Price.Value) * (decimal)1.05))
            {
                return Failed("单价超出范围");
            }

            return Success();
        }
    }
}