using AutoMapper;
using Nicole.Library.Models;
using Nicole.Library.Services;
using Nicole.Web.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using Nicole.Web.MapperHelper;

namespace Nicole.Web.Controllers.API
{
    public class StandardCostSettingController : BaseApiController
    {
        private readonly IProductService _productService;
        private readonly IStandardCostService _standardCostService;
        private readonly IMapperFactory _mapperFactory;
        public StandardCostSettingController(IProductService productService, IStandardCostService standardCostService, IMapperFactory mapperFactory)
        {
            _productService = productService;
            _standardCostService = standardCostService;
            _mapperFactory = mapperFactory;
        }
        public object Get([FromUri] StandardCostModel key, int pageIndex = 1)
        {
            var pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            var result =
                _standardCostService.GetStandardCosts()
                    .Where(n => (key.Remark == null || n.Remark.Contains(key.Remark.Trim())));
            if (key.ProductModel != null)
            {
                result =
                    result.Where(
                        n =>
                            (key.ProductModel.PartNumber == null ||
                             n.Product.PartNumber.Contains(key.ProductModel.PartNumber.Trim()))
                            && (key.ProductModel.Capacity == null ||
                                n.Product.Capacity.Contains(key.ProductModel.Capacity.Trim()))
                            && (key.ProductModel.Level == null ||
                                n.Product.Level.Contains(key.ProductModel.Level.Trim()))
                            && (key.ProductModel.Pitch == null ||
                                n.Product.Pitch.Contains(key.ProductModel.Pitch.Trim()))
                            && (key.ProductModel.SpecificDesign == null ||
                                n.Product.SpecificDesign.Contains(key.ProductModel.SpecificDesign.Trim()))
                            && (key.ProductModel.ProductType == null ||
                                n.Product.ProductType.Contains(key.ProductModel.ProductType.Trim()))
                            && (key.ProductModel.Voltage == null ||
                                n.Product.Voltage.Contains(key.ProductModel.Voltage.Trim())));

            }
            _mapperFactory.GetStandardCostMapper().Create();
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
