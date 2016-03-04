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
using Nicole.Web.Models.SearchModel;

namespace Nicole.Web.Controllers.API
{
    public class ApplyExpenseController : BaseApiController
    {
        private readonly IApplyExpenseService _applyExpenseService;
        private readonly IEmployeesService _employeesService;
        private readonly IMapperFactory _mapperFactory;
        public ApplyExpenseController(IApplyExpenseService applyExpenseService, IEmployeesService employeesService, IMapperFactory mapperFactory)
        {
            _applyExpenseService = applyExpenseService;
            _employeesService = employeesService;
            _mapperFactory = mapperFactory;
        }

        public object Get([FromUri] SearchApplyExpenseModel key, int pageIndex = 1)
        {
            var pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            var result =
                _applyExpenseService.GetApplyExpenses();
            result = result.Where(n => (key.Detail == null || n.Detail.Contains(key.Detail))
                                       && (key.IsApproved == null || n.IsApproved == key.IsApproved));
            if (key.ApplyExpenseTypeModel != null)
            {
                result = result.Where(n => n.ApplyExpenseTypeId == key.ApplyExpenseTypeModel.Id);
            }
            if (key.ConcernedPositionModel != null)
            {
                result = result.Where(n => n.ConcernedPositionId == key.ConcernedPositionModel.Id);
            }
            if (key.DateRangeModel != null)
            {
                result = result.Where(n => (n.Date >= key.DateRangeModel.From || key.DateRangeModel.From == null) &&
                                           (n.Date < key.DateRangeModel.To || key.DateRangeModel.To == null));
            }
            _mapperFactory.GetApplyExpenseMapper().Create();
            var model = new ApplyExpenseManagerModel
            {
                Models =
                    result
                        .OrderByDescending(n => n.Date)
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .Select(Mapper.Map<ApplyExpense, ApplyExpenseModel>)
                        .ToArray(),
                CurrentPageIndex = pageIndex,
                AllPage = (result.Count() / pageSize) + (result.Count() % pageSize == 0 ? 0 : 1)
            };
            return model;
        }
        public object Post(ApplyExpenseModel model)
        {
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var currentUser = HttpContext.Current.User.Identity.GetUser();
            var positionId = _employeesService.GetEmployee(currentUser.EmployeeId).EmployeePostions.Where(n => n.StartDate <= currentDate && (n.EndDate == null || n.EndDate >= currentDate) && n.IsDeleted == false).Select(n => n.Position.Id).FirstOrDefault();
            if (model == null)
            {
                return Failed("报销数据不能为空");
            }
            if (model.ConcernedPositionModel == null || model.ConcernedPositionModel.Id == Guid.Empty)
            {
                return Failed("找不到报销人");
            }
            if (model.Amount == null)
            {
                return Failed("必须填写报销金额");
            }
            if (model.ApplyExpenseTypeModel == null || model.ApplyExpenseTypeModel.Id == Guid.Empty)
            {
                return Failed("必须选择报销类型");
            }
            if (model.Date == null)
            {
                return Failed("必须选择报销日期");
            }
            try
            {
                _applyExpenseService.Insert(new ApplyExpense
                {
                    Id = Guid.NewGuid(),
                    Amount = model.Amount.Value,
                    ApplyExpenseTypeId = model.ApplyExpenseTypeModel.Id,
                    ConcernedPositionId = model.ConcernedPositionModel.Id,
                    Date = model.Date.Value,
                    Detail = model.Detail,
                    PositionId = positionId
                });
                return Success();
            }
            catch (Exception ex)
            {
                return Failed(ex.Message);
            }
        }

        public object Put(ApplyExpenseModel model)
        {
            if (model.ConcernedPositionModel == null || model.ConcernedPositionModel.Id == Guid.Empty)
            {
                return Failed("找不到报销人");
            }
            if (model.Amount == null)
            {
                return Failed("必须填写报销金额");
            }
            if (model.ApplyExpenseTypeModel == null || model.ApplyExpenseTypeModel.Id == Guid.Empty)
            {
                return Failed("必须选择报销类型");
            }
            if (model.Date == null)
            {
                return Failed("必须选择报销日期");
            }
            var item = _applyExpenseService.GetApplyExpense(model.Id);
            if (item == null)
            {
                return Failed("找不到报销数据");
            }
            if (item.IsApproved)
            {
                return Failed("已通过审核禁止修改");
            }
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var currentUser = HttpContext.Current.User.Identity.GetUser();
            var positionId = _employeesService.GetEmployee(currentUser.EmployeeId).EmployeePostions.Where(n => n.StartDate <= currentDate && (n.EndDate == null || n.EndDate >= currentDate) && n.IsDeleted == false).Select(n => n.Position.Id).FirstOrDefault();
            try
            {
                item.ApplyExpenseTypeId = model.ApplyExpenseTypeModel.Id;
                item.Amount = model.Amount.Value;
                item.ConcernedPositionId = model.ConcernedPositionModel.Id;
                item.Detail = model.Detail;
                item.Date = model.Date.Value;
                item.IsApproved = model.IsApproved;
                item.PositionId = positionId;
                _applyExpenseService.Update();
                return Success();
            }
            catch (Exception ex)
            {
                return Failed(ex.Message);
            }

        }

        public object Delete(Guid id)
        {
            var item = _applyExpenseService.GetApplyExpense(id);
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var currentUser = HttpContext.Current.User.Identity.GetUser();
            var positionId = _employeesService.GetEmployee(currentUser.EmployeeId).EmployeePostions.Where(n => n.StartDate <= currentDate && (n.EndDate == null || n.EndDate >= currentDate) && n.IsDeleted == false).Select(n => n.Position.Id).FirstOrDefault();
            if (item == null)
            {
                return Failed("找不到报销数据");
            }

            try
            {
                item.IsDeleted = true;
                item.PositionId = positionId;
                _applyExpenseService.Update();
                return Success();
            }
            catch (Exception ex)
            {
                return Failed(ex.Message);
            }

        }
    }
}
