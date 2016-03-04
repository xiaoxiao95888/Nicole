using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Nicole.Library.Models;
using Nicole.Library.Services;
using Nicole.Web.Infrastructure;
using Nicole.Web.MapperHelper;
using Nicole.Web.Models;

namespace Nicole.Web.Controllers.API
{
    public class FaPiaoController : BaseApiController
    {
        private readonly IMapperFactory _mapperFactory;
        private readonly IFaPiaoService _fapiaoService;
        private readonly IFinanceService _financeService;
        private readonly IEmployeesService _employeesService;
        public FaPiaoController(IMapperFactory mapperFactory, IFaPiaoService fapiaoService, IFinanceService financeService, IEmployeesService employeesService)
        {
            _mapperFactory = mapperFactory;
            _fapiaoService = fapiaoService;
            _financeService = financeService;
            _employeesService = employeesService;
        }

        public object Post(FaPiaoModel model)
        {
            var finance = _financeService.GetFinance(model.FinanceId);
            if (finance == null)
            {
                return Failed("找不到收款");
            }
            if (string.IsNullOrEmpty(model.Code))
            {
                return Failed("发票编号不能为空");
            }
            if (model.Code.Trim().Length != 8)
            {
                return Failed("发票编号只能是8位");
            }
            if (_fapiaoService.GetFaPiaos().Any(p => p.Code == model.Code.Trim()))
            {
                return Failed("发票编号重复");
            }
            try
            {
                _fapiaoService.Insert(new FaPiao
                {
                    Id = Guid.NewGuid(),
                    Code = model.Code.Trim(),
                    FinanceId = model.FinanceId
                });
                return Success();
            }
            catch (Exception ex)
            {
                return Failed(ex.Message);
            }

        }

        public object Put(FaPiaoModel model)
        {
            var fapiao = _fapiaoService.GetFaPiao(model.Id);
            if (fapiao == null)
            {
                return Failed("找不到发票");
            }
            if (string.IsNullOrEmpty(model.Code))
            {
                return Failed("发票编号不能为空");
            }
            if (model.Code.Trim().Length != 8)
            {
                return Failed("发票编号只能是8位");
            }
            try
            {
                fapiao.Code = model.Code.Trim();
                _fapiaoService.Update();
                return Success();
            }
            catch (Exception ex)
            {

                return Failed(ex.Message);
            }
        }
        public object Delete(Guid id)
        {
            var item = _fapiaoService.GetFaPiao(id);
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var currentPosition =
                _employeesService.GetEmployee(HttpContext.Current.User.Identity.GetUser().EmployeeId)
                    .EmployeePostions.Where(
                        n => n.StartDate <= currentDate && (n.EndDate == null || n.EndDate >= currentDate) && n.IsDeleted == false)
                    .Select(n => n.Position)
                    .FirstOrDefault();
            if (item == null)
            {
                return Failed("找不到发票");
            }
            if (currentPosition == null || item.Finance.PositionId != currentPosition.Id)
            {
                return Failed("没有权限");
            }
            item.IsDeleted = true;
            try
            {
                var allfapiao = item.Finance.FaPiaos;
                if (allfapiao.All(p => p.IsDeleted))
                {
                    item.Finance.HasFaPiao = false;
                }
                _fapiaoService.Update();
                return Success();
            }
            catch (Exception ex)
            {
                return Failed(ex.Message);
            }
        }
    }
}
