using AutoMapper;
using Nicole.Library.Models;
using Nicole.Library.Services;
using Nicole.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nicole.Web.Controllers.API
{
    public class ProductSettingController : BaseApiController
    {
        private readonly IProductTypeService _productTypeService;

        public ProductSettingController(IProductTypeService productTypeService)
        {
            _productTypeService = productTypeService;
        }
        // GET: ProductSetting
        public object Get()
        {
            Mapper.Reset();
            Mapper.CreateMap<Product, ProductModel>();
            Mapper.CreateMap<ProductType, ProductTypeModel>().ForMember(n => n.ProductModels, opt => opt.MapFrom(src => src.Products));

            return _productTypeService.GeProductTypes().Select(Mapper.Map<ProductType, ProductTypeModel>);
        }
        public object Post(ProductTypeModel model)
        {
            var item = new ProductType
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Voltage = model.Voltage,
                Capacity = model.Capacity,
                Pitch = model.Pitch,
                Level = model.Level
            };
            try
            {
                _productTypeService.Insert(item);
            }
            catch (Exception ex)
            {
                return Failed(ex.Message);
            }
            return Success();
        }
        public object Put(ProductTypeModel model)
        {
            var item = _productTypeService.GetProductType(model.Id);
            item.Capacity = model.Capacity;
            item.Level = model.Level;
            item.Name = model.Name;
            item.Pitch = model.Pitch;
            item.Voltage = model.Voltage;
            try
            {
                _productTypeService.Update();
            }
            catch (Exception ex)
            {
                return Failed(ex.Message);
            }
            return Success();

        }
    }
}