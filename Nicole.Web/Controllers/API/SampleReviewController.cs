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
    public class SampleReviewController : BaseApiController
    {
        private readonly ISampleService _sampleService;
        private readonly ISampleReviewService _sampleReviewService;
        private readonly IMapperFactory _mapperFactory;
        private readonly IEmployeesService _employeesService;
        private readonly IPositionService _positionService;
        private readonly IAuditLevelService _auditLevelService;

        public SampleReviewController(ISampleService sampleService, IMapperFactory mapperFactory,
            IEmployeesService employeesService, IPositionService positionService, IAuditLevelService auditLevelService,
            ISampleReviewService sampleReviewService)
        {
            _sampleService = sampleService;
            _mapperFactory = mapperFactory;
            _employeesService = employeesService;
            _positionService = positionService;
            _auditLevelService = auditLevelService;
            _sampleReviewService = sampleReviewService;
        }

        /// <summary>
        /// 通过审核
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public object Post(SampleReviewModel model)
        {
            var item = _sampleReviewService.GetSampleReview(model.Id);
            if (item == null)
            {
                return Failed("找不到审核记录");
            }

            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var currentPosition =
                _employeesService.GetEmployee(HttpContext.Current.User.Identity.GetUser().EmployeeId)
                    .EmployeePostions.Where(
                        n => n.StartDate <= currentDate && (n.EndDate == null || n.EndDate >= currentDate) && n.IsDeleted == false)
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

            //var parentRole =
            //    _auditLevelService.GetAuditLevels()
            //        .Where(n => n.RoleId == currentPosition.Role.Id)
            //        .Select(n => n.ParentRole).FirstOrDefault();
            //最终审核
            if (currentPosition.Parent == null)
            {
                item.Sample.IsApproved = true;
                item.IsDeleted = true;
            }
            //else
            //{
            //    item.SendToRoleId = parentRole.Id;
            //}
            try
            {
                _sampleReviewService.Update();
                return Success();
            }
            catch (Exception ex)
            {
                return Failed(ex.Message);
            }

        }
        public object Get([FromUri]SearchSampleModel key, int pageIndex = 1)
        {
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var currentPosition =
                _employeesService.GetEmployee(HttpContext.Current.User.Identity.GetUser().EmployeeId)
                    .EmployeePostions.Where(
                        n => n.StartDate <= currentDate && (n.EndDate == null || n.EndDate >= currentDate) && n.IsDeleted == false)
                    .Select(n => n.Position)
                    .FirstOrDefault();
            var pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            var result =
                _sampleService.GetSamples()
                    .Where(
                        n =>
                            n.SampleReviews.OrderByDescending(p => p.CreatedTime)
                                .FirstOrDefault(
                                    p => p.IsReturn == false && p.Sample.IsApproved == false && p.IsDeleted == false)
                                .SendToRoleId ==
                            currentPosition.RoleId);

            result = result.Where(n => (key.CustomerCode == null || n.Customer.Code == key.CustomerCode.Trim())
                                       && (key.IsApproved == null || n.IsApproved == key.IsApproved)
                                        && (key.CustomerName == null || n.Customer.Name.Contains(key.CustomerName))
                                       &&
                                       (key.PartNumber == null ||
                                        n.Product.PartNumber.Trim().ToUpper() == key.PartNumber.Trim().ToUpper())
                                        &&
                                       (key.Code == null ||
                                        n.Code.Trim() == key.Code.Trim())
                                       &&
                                       (key.PositionId == null || key.PositionId == Guid.Empty ||
                                        n.PositionId == key.PositionId));

            _mapperFactory.GetSampleMapper().Create();
            var model = new SampleSettingModel
            {
                Models =
                    result
                        .OrderByDescending(n => n.Code)
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .Select(Mapper.Map<Sample, SampleModel>)
                        .ToArray(),
                CurrentPageIndex = pageIndex,
                AllPage = (result.Count() / pageSize) + (result.Count() % pageSize == 0 ? 0 : 1)
            };
            return model;
        }
        /// <summary>
        /// 退回样品审核
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public object Put(SampleReviewModel model)
        {
            var item = _sampleReviewService.GetSampleReview(model.Id);
            if (item == null)
            {
                return Failed("找不到审核记录");
            }
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var currentPosition =
                _employeesService.GetEmployee(HttpContext.Current.User.Identity.GetUser().EmployeeId)
                    .EmployeePostions.Where(
                        n => n.StartDate <= currentDate && (n.EndDate == null || n.EndDate >= currentDate) && n.IsDeleted == false)
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
                _sampleReviewService.Update();
                return Success();
            }
            catch (Exception ex)
            {
                return Failed(ex.Message);
            }
        }

    }
}
