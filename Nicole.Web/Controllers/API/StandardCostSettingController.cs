using AutoMapper;
using Nicole.Library.Models;
using Nicole.Library.Services;
using Nicole.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Nicole.Web.Controllers.API
{
    public class StandardCostSettingController : BaseApiController
    {
        private readonly IProductService _productService;
        private readonly IStandardCostService _standardCostService;
        public StandardCostSettingController(IProductService productService, IStandardCostService standardCostService)
        {
            _productService = productService;
            _standardCostService = standardCostService;
        }
        public object Get()
        {
            var pageIndex = string.IsNullOrEmpty(HttpContext.Current.Request["pageIndex"])
                ? 1
                : Convert.ToInt32(HttpContext.Current.Request["pageIndex"]);
            var pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            var productTypeKey = HttpContext.Current.Request["ProductTypeKey"] ?? string.Empty;
            var voltageKey = HttpContext.Current.Request["VoltageKey"] ?? string.Empty;
            var capacityKey = HttpContext.Current.Request["CapacityKey"] ?? string.Empty;
            var pitchKey = HttpContext.Current.Request["PitchKey"] ?? string.Empty;
            var levelKey = HttpContext.Current.Request["LevelKey"] ?? string.Empty;
            var partNumberKey = HttpContext.Current.Request["PartNumberKey"] ?? string.Empty;
            var specificDesignKey = HttpContext.Current.Request["SpecificDesignKey"] ?? string.Empty;
            var quotedTimeKey = HttpContext.Current.Request["QuotedTimeKey"] ?? string.Empty;
            var result = _standardCostService.GetStandardCosts();
            if (!string.IsNullOrEmpty(quotedTimeKey))
            {
                var quotedTime = Convert.ToDateTime(quotedTimeKey);
                result = result.Where(n => n.QuotedTime == quotedTime);
            }
            if (!string.IsNullOrEmpty(productTypeKey))
            {
                result = result.Where(n => n.Product.ProductType.Contains(productTypeKey));
            }
            if (!string.IsNullOrEmpty(voltageKey))
            {
                result = result.Where(n => n.Product.Voltage.Contains(voltageKey));
            }
            if (!string.IsNullOrEmpty(capacityKey))
            {
                result = result.Where(n => n.Product.Capacity.Contains(capacityKey));
            }
            if (!string.IsNullOrEmpty(pitchKey))
            {
                result = result.Where(n => n.Product.Pitch.Contains(pitchKey));
            }
            if (!string.IsNullOrEmpty(levelKey))
            {
                result = result.Where(n => n.Product.Level.Contains(levelKey));
            }
            if (!string.IsNullOrEmpty(specificDesignKey))
            {
                result = result.Where(n => n.Product.SpecificDesign.Contains(specificDesignKey));
            }
            if (!string.IsNullOrEmpty(partNumberKey))
            {
                result = result.Where(n => n.Product.PartNumber.Contains(partNumberKey));
            }
            Mapper.Reset();
            Mapper.CreateMap<Product, ProductModel>();
            Mapper.CreateMap<StandardCost, StandardCostModel>().ForMember(n => n.ProductModel, opt => opt.MapFrom(src => src.Product));
            var model = new StandardCostSettingModel
            {
                StandardCostModels =
                    result
                        .OrderByDescending(n => n.CreatedTime)
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .Select(Mapper.Map<StandardCost, StandardCostModel>)
                        .ToArray(),
                CurrentPageIndex = pageIndex,
                AllPage = (result.Count() / pageSize) + (result.Count() % pageSize == 0 ? 0 : 1)
            };
            return model;

        }
        public object Get(Guid id)
        {
            Mapper.Reset();
            Mapper.CreateMap<Product, ProductModel>();
            Mapper.CreateMap<StandardCost, StandardCostModel>().ForMember(n => n.ProductModel, opt => opt.MapFrom(src => src.Product));
            return Mapper.Map<StandardCost, StandardCostModel>(_standardCostService.GetStandardCost(id));
        }
        public object Post(StandardCostModel model)
        {
            var errormessage = string.Empty;
            if (model == null)
            {
                errormessage = "报价不得为空";
            }
            else if (model.ProductModel == null || string.IsNullOrEmpty(model.ProductModel.PartNumber))
            {
                errormessage = "料号不能为空";
            }
            else if (model.QuotedTime == DateTime.MinValue)
            {
                errormessage = "报价日期不能为空";
            }
            else if (_standardCostService.GetStandardCosts().Any(n => n.Product.PartNumber == model.ProductModel.PartNumber && n.QuotedTime == model.QuotedTime))
            {
                errormessage = "报价不能重复";
            }

            if (string.IsNullOrEmpty(errormessage))
            {
                var product = _productService.GetProducts().FirstOrDefault(n => n.PartNumber == model.ProductModel.PartNumber);
                if (product == null || product.IsDeleted)
                {
                    errormessage = "找不到料号或料号被删除";
                }
                else
                {
                    var item = new StandardCost
                    {
                        Id = Guid.NewGuid(),
                        ProductId = product.Id,
                        Price = model.Price,
                        QuotedTime = model.QuotedTime,
                        Remark = model.Remark
                    };
                    try
                    {
                        _standardCostService.Insert(item);
                    }
                    catch (Exception ex)
                    {
                        return Failed(ex.Message);
                    }
                }
                
            }

            return string.IsNullOrEmpty(errormessage) ? Success() : Failed(errormessage);
        }
        public object Put(StandardCostModel model)
        {
            var errormessage = string.Empty;
            if (model == null)
            {
                errormessage = "报价不得为空";
            }
            else if (model.ProductModel.Id == Guid.Empty)
            {
                errormessage = "产品不能为空";
            }
            else if (model.QuotedTime == DateTime.MinValue)
            {
                errormessage = "报价日期不能为空";
            }
            else
            {
                var item = _standardCostService.GetStandardCost(model.Id);
                if (item.IsDeleted)
                {
                    errormessage = "该报价已删除";
                }
                if (string.IsNullOrEmpty(errormessage))
                {
                    item.ProductId = model.ProductModel.Id;
                    item.Price = model.Price;
                    item.QuotedTime = model.QuotedTime;
                    item.Remark = model.Remark;
                    try
                    {
                        _standardCostService.Update();
                    }
                    catch (Exception ex)
                    {
                        errormessage = ex.Message;
                    }
                }
            }
            return string.IsNullOrEmpty(errormessage) ? Success() : Failed(errormessage);

        }
        public object Delete(Guid id)
        {
            try
            {

                _standardCostService.Delete(id);
            }
            catch (Exception ex)
            {
                return Failed(ex.Message);
            }


            return Success();
        }
    }
}
