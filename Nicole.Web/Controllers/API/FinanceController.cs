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
    public class FinanceController : BaseApiController
    {
        private readonly IMapperFactory _mapperFactory;
        private readonly IOrderService _orderService;
        private readonly IFinanceService _financeService;
        private readonly IFaPiaoService _faPiaoService;
        private readonly IEmployeesService _employeesService;
        public FinanceController(IMapperFactory mapperFactory, IOrderService orderService, IFinanceService financeService, IEmployeesService employeesService, IFaPiaoService faPiaoService)
        {
            _mapperFactory = mapperFactory;
            _orderService = orderService;
            _financeService = financeService;
            _employeesService = employeesService;
            _faPiaoService = faPiaoService;
        }
        public object Get([FromUri]FinancePageModel key, int pageIndex = 1)
        {
            var pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            var result =
                _orderService.GetOrders()
                    .Where(n => n.IsApproved && n.IsDeleted == false);
            if (key.CustomerModel != null)
            {
                result =
                       result.Where(
                           n =>
                               (
                                   key.CustomerModel.Code == null ||
                                   n.Enquiry.Customer.Code.Contains(key.CustomerModel.Code.Trim()))
                               && (
                                   key.CustomerModel.Name == null ||
                                   n.Enquiry.Customer.Name.Contains(key.CustomerModel.Name.Trim())));
            }
            if (key.OrderModel != null)
            {
                result =
                    result.Where(
                        n =>

                            key.OrderModel.Code == null ||
                            n.Code.Contains(key.OrderModel.Code.Trim()));

            }

            _mapperFactory.GetFinanceMapper().OrderToFinacePage();
            var model = new FinanceManagerModel
            {
                Models =
                   result.OrderBy(n => n.Finances.Where(p => !p.IsDeleted).Sum(p => p.Amount))
                       .ThenBy(n => n.LastPayDate).ThenByDescending(n => n.UpdateTime)
                       .Skip((pageIndex - 1) * pageSize)
                       .Take(pageSize)
                       .Select(Mapper.Map<Order, FinancePageModel>)
                       .ToArray(),
                CurrentPageIndex = pageIndex,
                AllPage = (result.Count() / pageSize) + (result.Count() % pageSize == 0 ? 0 : 1)
            };
            return model;
        }
        public object Get(Guid id)
        {
            var result =
                _financeService.GetFinance(id);
            _mapperFactory.GetFinanceMapper().Create();
            return Mapper.Map<Finance, FinanceModel>(result);
        }
        

        public object Post(FinanceModel model)
        {
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var currentPosition =
                _employeesService.GetEmployee(HttpContext.Current.User.Identity.GetUser().EmployeeId)
                    .EmployeePostions.Where(
                        n => n.StartDate <= currentDate && (n.EndDate == null || n.EndDate >= currentDate))
                    .Select(n => n.Position)
                    .FirstOrDefault();
            if (currentPosition == null)
            {
                return Failed("提交失败");
            }
            if (model == null)
            {
                return Failed("提交失败");
            }
            if (model.Amount == null)
            {
                return Failed("金额不得为空");
            }
            if (model.HasFaPiao)
            {
                if (string.IsNullOrEmpty(model.FaPiaoNumbers))
                {
                    return Failed("发票编号不得为空");
                }
            }
            if (model.PayDate == null)
            {
                return Failed("收款日期不得为空");
            }
            try
            {
                var finance = new Finance
                {
                    Id = Guid.NewGuid(),
                    Amount = model.Amount.Value,
                    HasFaPiao = model.HasFaPiao,
                    FaPiaos = model.HasFaPiao ? new List<FaPiao>() : null,
                    OrderId = model.OrderId,
                    PayDate = model.PayDate.Value,
                    PositionId = currentPosition.Id,
                    Remark = model.Remark
                };

                if (model.HasFaPiao)
                {
                    var fapiaonumbers = model.FaPiaoNumbers.Split(',').ToArray();
                    if (!fapiaonumbers.Any())
                    {
                        return Failed("发票编号不得为空");
                    }
                    if (fapiaonumbers.Any(p => p.Trim().Length != 8))
                    {
                        return Failed("发票编号必须得8位");
                    }
                    if (fapiaonumbers.Distinct().Count()!=fapiaonumbers.Length)
                    {
                        return Failed("发票编号不得重复");
                    }
                    if (_faPiaoService.GetFaPiaos().Any(p => fapiaonumbers.Contains(p.Code)))
                    {
                        return Failed("发票编号已存在，不得重复");
                    }
                    foreach (var item in fapiaonumbers)
                    {
                        finance.FaPiaos.Add(new FaPiao
                        {
                            Id = Guid.NewGuid(),
                            Code = item.Trim()
                        });
                    }
                }
                _financeService.Insert(finance);
                return Success();
            }
            catch (Exception ex)
            {

                return Failed(ex.Message);
            }
        }
        public object Delete(Guid id)
        {
            var item = _financeService.GetFinance(id);
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var currentPosition =
                _employeesService.GetEmployee(HttpContext.Current.User.Identity.GetUser().EmployeeId)
                    .EmployeePostions.Where(
                        n => n.StartDate <= currentDate && (n.EndDate == null || n.EndDate >= currentDate))
                    .Select(n => n.Position)
                    .FirstOrDefault();
            if (item == null)
            {
                return Failed("找不到收款");
            }
            if (currentPosition == null || item.PositionId != currentPosition.Id)
            {
                return Failed("没有权限");
            }
            item.IsDeleted = true;
            try
            {
                _financeService.Update();
                return Success();
            }
            catch (Exception ex)
            {
                return Failed(ex.Message);
            }
        }
    }
    public class FinanceByOrderController : BaseApiController
    {
        private readonly IMapperFactory _mapperFactory;

        private readonly IFinanceService _financeService;

        public FinanceByOrderController(IMapperFactory mapperFactory, IFinanceService financeService)
        {
            _mapperFactory = mapperFactory;
            _financeService = financeService;

        }
        /// <summary>
        /// orderId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object Get(Guid id)
        {
            var result =
                _financeService.GetFinances().Where(n => n.OrderId == id).OrderByDescending(n => n.PayDate);
            _mapperFactory.GetFinanceMapper().Create();
            return result.Select(Mapper.Map<Finance, FinanceModel>);
        }


    }
}
