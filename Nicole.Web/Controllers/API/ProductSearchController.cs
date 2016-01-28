using Nicole.Library.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Nicole.Library.Models;
using Nicole.Web.Models;

namespace Nicole.Web.Controllers.API
{
    public class ProductSearchController : BaseApiController
    {
        private readonly IProductService _productService;

        public ProductSearchController(IProductService productService)
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
            Mapper.CreateMap<Product, ProductModel>().ForMember(n=>n.Price,opt=>opt.Ignore());
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
        public object Get(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var model = _productService.GetProducts().FirstOrDefault(n => n.PartNumber.Contains(id.ToLower().Trim()));
                if (model == null)
                {
                    return Failed("找不到相关料号，请完善数据");
                }
                else
                {
                    Mapper.Reset();
                    Mapper.CreateMap<Product, ProductModel>().ForMember(n => n.Price, opt => opt.Ignore());
                    return Mapper.Map<Product, ProductModel>(model);
                }
            }
            return Failed("找不到相关料号，请完善数据");
        }
    }
}
