using AutoMapper;
using Nicole.Library.Models;
using Nicole.Library.Services;
using Nicole.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
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
        public object Get([FromUri] ProductModel key, int pageIndex = 1)
        {
            var pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            var result = _productService.GetProducts().Where(n =>
                (key.PartNumber == null ||
                 n.PartNumber.Contains(key.PartNumber.Trim()))
                && (key.Capacity == null ||
                    n.Capacity.Contains(key.Capacity.Trim()))
                && (key.Level == null ||
                    n.Level.Contains(key.Level.Trim()))
                && (key.Pitch == null ||
                    n.Pitch.Contains(key.Pitch.Trim()))
                && (key.SpecificDesign == null ||
                    n.SpecificDesign.Contains(key.SpecificDesign.Trim()))
                && (key.ProductType == null ||
                    n.ProductType.Contains(key.ProductType.Trim()))
                    && (key.Price == null ||
                    n.Price == key.Price)
                && (key.Voltage == null ||
                    n.Voltage.Contains(key.Voltage.Trim())));

            Mapper.Reset();
            Mapper.CreateMap<Product, ProductModel>();
            var model = new ProductSettingModel
            {
                ProductModels =
                    result
                        .OrderByDescending(n => n.UpdateTime)
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .Select(Mapper.Map<Product, ProductModel>)
                        .ToArray(),
                CurrentPageIndex = pageIndex,
                AllPage = (result.Count() / pageSize) + (result.Count() % pageSize == 0 ? 0 : 1)
            };
            return model;

        }
        public object Post(ProductModel model)
        {
            if (model == null)
            {
                return Failed("产品不能为空");
            }
            if (string.IsNullOrEmpty(model.PartNumber))
            {
                return Failed("料号不能为空");
            }
            if (_productService.GetProducts().Any(n => n.PartNumber == model.PartNumber.Trim()))
            {

                return Failed("料号不能重复");
            }
            var item = new Product
            {
                Id = Guid.NewGuid(),
                PartNumber = model.PartNumber.Trim().ToUpper(),
                ProductType = string.IsNullOrEmpty(model.ProductType) ? model.ProductType : model.ProductType.Trim().ToUpper(),
                Voltage = string.IsNullOrEmpty(model.Voltage) ? model.Voltage : model.Voltage.Trim(),
                Capacity = string.IsNullOrEmpty(model.Capacity) ? model.Capacity : model.Capacity.Trim(),
                Pitch = string.IsNullOrEmpty(model.Pitch) ? model.Pitch : model.Pitch.Trim(),
                Level = string.IsNullOrEmpty(model.Level) ? model.Level : model.Level.Trim(),
                SpecificDesign =
                    string.IsNullOrEmpty(model.SpecificDesign) ? model.SpecificDesign : model.SpecificDesign.Trim(),
                Price = model.Price
            };
            try
            {
                _productService.Insert(item);
            }
            catch (Exception ex)
            {
                return Failed(ex.Message);
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