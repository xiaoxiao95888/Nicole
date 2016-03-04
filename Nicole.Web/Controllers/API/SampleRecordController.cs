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
    public class SampleRecordController : BaseApiController
    {
        private readonly ISampleService _sampleService;
        private readonly IMapperFactory _mapperFactory;
        public SampleRecordController(ISampleService sampleService, IMapperFactory mapperFactory)
        {
            _sampleService = sampleService;
            _mapperFactory = mapperFactory;
        }
       
        public object Get([FromUri]SearchSampleModel key, int pageIndex = 1)
        {
            var pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            var result =
                _sampleService.GetSamples();

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
                                        n.PositionId == key.PositionId)
                                         &&
                                       (key.Id == null || key.Id == Guid.Empty ||
                                        n.Id == key.Id));

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
    }
}
