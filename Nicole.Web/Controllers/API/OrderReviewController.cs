using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using AutoMapper;
using Nicole.Library.Models;
using Nicole.Library.Services;
using Nicole.Web.Infrastructure;
using Nicole.Web.MapperHelper;
using Nicole.Web.Models;

namespace Nicole.Web.Controllers.API
{
    public class OrderReviewController : BaseApiController
    {
        private readonly IOrderReviewService _orderReviewService;
        private readonly IAuditLevelService _auditLevelService;
        private readonly IEmployeesService _employeesService;
        private readonly IMapperFactory _mapperFactory;
        private readonly IOrderService _orderService;
        public OrderReviewController(IOrderReviewService orderReviewService, IAuditLevelService auditLevelService, IEmployeesService employeesService, IMapperFactory mapperFactory, IOrderService orderService)
        {
            _orderReviewService = orderReviewService;
            _auditLevelService = auditLevelService;
            _employeesService = employeesService;
            _orderService = orderService;
            _mapperFactory = mapperFactory;
        }
        public object Get([FromUri]OrderModel key, int pageIndex = 1)
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
                _orderService.GetOrders()
                    .Where(
                        n =>
                            n.OrderReviews.OrderByDescending(p => p.CreatedTime).FirstOrDefault(p => p.IsReturn == false && p.Order.IsApproved == false && p.IsDeleted == false).SendToRoleId ==
                            currentPosition.RoleId);
           
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

            var result =
                _orderService
                    .GetOrders()
                    .FirstOrDefault(
                        n =>
                            n.Id == id &&
                            n.OrderReviews.OrderByDescending(p => p.CreatedTime)
                                .FirstOrDefault(p => p.SendToRoleId == currentPosition.RoleId) != null);

            _mapperFactory.GetOrderMapper().Create();
            return Mapper.Map<Order, OrderModel>(result);
        }
        /// <summary>
        /// 通过审核
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public object Post(OrderReviewModel model)
        {
            var item = _orderReviewService.GetOrderReview(model.Id);
            if (item == null)
            {
                return Failed("找不到合同");
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
            if (item.SendToRoleId != currentPosition.RoleId)
            {
                return Failed("没有权限");
            }

            var parentRole =
                _auditLevelService.GetAuditLevels()
                    .Where(n => n.RoleId == currentPosition.Role.Id)
                    .Select(n => n.ParentRole).FirstOrDefault();
            //最终审核
            if (parentRole == null)
            {
                item.Order.IsApproved = true;
                item.IsDeleted = true;
            }
            else
            {
                item.SendToRoleId = parentRole.Id;
            }
            try
            {
                _orderReviewService.Update();
                return Success();
            }
            catch (Exception ex)
            {
                return Failed(ex.Message);
            }
        }
        /// <summary>
        /// 退回合同
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public object Put(OrderReviewModel model)
        {
            var item = _orderReviewService.GetOrderReview(model.Id);
            if (item == null)
            {
                return Failed("找不到合同");
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
            if (item.SendToRoleId != currentPosition.RoleId)
            {
                return Failed("没有权限");
            }
            try
            {
                item.IsReturn = true;
                item.ReturnComments = model.ReturnComments;
                _orderReviewService.Update();
                return Success();
            }
            catch (Exception ex)
            {
                return Failed(ex.Message);
            }
        }
    }
}