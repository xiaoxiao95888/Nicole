using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nicole.Library.Services;
using Nicole.Web.Infrastructure;
using Nicole.Web.Models;

namespace Nicole.Web.Controllers.API
{
    public class OrderReviewController : BaseApiController
    {
        private readonly IOrderReviewService _orderReviewService;
        private readonly IAuditLevelService _auditLevelService;
        private readonly IEmployeesService _employeesService;
        public OrderReviewController(IOrderReviewService orderReviewService, IAuditLevelService auditLevelService, IEmployeesService employeesService)
        {
            _orderReviewService = orderReviewService;
            _auditLevelService = auditLevelService;
            _employeesService = employeesService;
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
            if (item.SendToPositionId != currentPosition.Id)
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
                item.Orders.IsApproved = true;
                item.IsDeleted = true;
            }
            else
            {
                var sendtoPosition = parentRole.Positions.FirstOrDefault();
                if (sendtoPosition == null)
                {
                    return Failed("找不到审核人");
                }
                item.SendToPositionId = sendtoPosition.Id;
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
        public object Put( OrderReviewModel model)
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
            if (item.SendToPositionId != currentPosition.Id)
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