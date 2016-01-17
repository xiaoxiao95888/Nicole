using AutoMapper;
using Nicole.Library.Models;
using Nicole.Library.Services;
using Nicole.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nicole.Web.Controllers.API
{
    public class ProductSettingController : BaseApiController
    {
        private readonly IProductService _productService;

        public ProductSettingController(IProductService productService)
        {
            _productService = productService;
        }
        // GET: ProductSetting
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
            var result = _productService.GetProducts();
            if (!string.IsNullOrEmpty(productTypeKey))
            {
                result = result.Where(n => n.ProductType.Contains(productTypeKey));
            }
            if (!string.IsNullOrEmpty(voltageKey))
            {
                result = result.Where(n => n.Voltage.Contains(voltageKey));
            }
            if (!string.IsNullOrEmpty(capacityKey))
            {
                result = result.Where(n => n.Capacity.Contains(capacityKey));
            }
            if (!string.IsNullOrEmpty(pitchKey))
            {
                result = result.Where(n => n.Pitch.Contains(pitchKey));
            }
            if (!string.IsNullOrEmpty(levelKey))
            {
                result = result.Where(n => n.Level.Contains(levelKey));
            }
            if (!string.IsNullOrEmpty(specificDesignKey))
            {
                result = result.Where(n => n.SpecificDesign.Contains(specificDesignKey));
            }
            if (!string.IsNullOrEmpty(partNumberKey))
            {
                result = result.Where(n => n.PartNumber.Contains(partNumberKey));
            }
            Mapper.Reset();
            Mapper.CreateMap<Product, ProductModel>();
            var model = new ProductSettingModel
            {
                ProductModels =
                    result
                        .OrderByDescending(n => n.CreatedTime)
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .Select(Mapper.Map<Product, ProductModel>)
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
            return Mapper.Map<Product, ProductModel>(_productService.GetProduct(id));
        }
        public object Post(ProductModel model)
        {
            var errormessage = string.Empty;
            if (model == null)
            {
                errormessage = "型号不能为空";
            }
            else if (string.IsNullOrEmpty(model.ProductType))
            {
                errormessage = "型号不能为空";
            }
            else if (string.IsNullOrEmpty(model.PartNumber))
            {
                errormessage = "料号不能为空";
            }
            else if (_productService.GetProducts().Any(n => n.PartNumber == model.PartNumber.Trim()))
            {
                errormessage = "料号不能重复";
            }

            if (string.IsNullOrEmpty(errormessage))
            {
                var item = new Product
                {
                    Id = Guid.NewGuid(),
                    PartNumber = model.PartNumber.Trim(),
                    ProductType = model.ProductType.Trim(),
                    Voltage = string.IsNullOrEmpty(model.Voltage) ? model.Voltage : model.Voltage.Trim(),
                    Capacity = string.IsNullOrEmpty(model.Capacity) ? model.Capacity : model.Capacity.Trim(),
                    Pitch = string.IsNullOrEmpty(model.Pitch) ? model.Pitch : model.Pitch.Trim(),
                    Level = string.IsNullOrEmpty(model.Level) ? model.Level : model.Level.Trim(),
                    SpecificDesign = string.IsNullOrEmpty(model.SpecificDesign) ? model.SpecificDesign : model.SpecificDesign.Trim(),
                };
                try
                {
                    _productService.Insert(item);
                }
                catch (Exception ex)
                {
                    return Failed(ex.Message);
                }
            }
            else
            {
                return Failed(errormessage);

            }
            return Success();
        }
        public object Put(ProductModel model)
        {
            var errormessage = string.Empty;
            if (model == null)
            {
                errormessage = "型号不能为空";
            }
            else if (string.IsNullOrEmpty(model.ProductType))
            {
                errormessage = "型号不能为空";
            }
            else if (string.IsNullOrEmpty(model.PartNumber))
            {
                errormessage = "料号不能为空";
            }
            else
            {
                var item = _productService.GetProduct(model.Id);
                if (item.IsDeleted)
                {
                    errormessage = "产品已删除";
                }
                if (string.IsNullOrEmpty(errormessage))
                {
                    item.PartNumber = model.PartNumber.Trim();
                    item.ProductType = model.ProductType.Trim();
                    item.Capacity = string.IsNullOrEmpty(model.Capacity) ? model.Capacity : model.Capacity.Trim();
                    item.Level = string.IsNullOrEmpty(model.Level) ? model.Level : model.Level.Trim();
                    item.ProductType = string.IsNullOrEmpty(model.ProductType) ? model.ProductType : model.ProductType.Trim();
                    item.Pitch = string.IsNullOrEmpty(model.Pitch) ? model.Pitch : model.Pitch.Trim();
                    item.Voltage = string.IsNullOrEmpty(model.Voltage) ? model.Voltage : model.Voltage.Trim();
                    item.SpecificDesign = string.IsNullOrEmpty(model.SpecificDesign) ? model.SpecificDesign : model.SpecificDesign.Trim();
                    try
                    {
                        _productService.Update();
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

                _productService.Delete(id);
            }
            catch (Exception ex)
            {
                return Failed(ex.Message);
            }


            return Success();
        }
    }
}