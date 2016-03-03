using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Nicole.Library.Models;
using Nicole.Web.MapperHelper.IMapperInterfaces;
using Nicole.Web.Models;

namespace Nicole.Web.MapperHelper
{
    public class StandardCostMapper : IStandardCostMapper
    {
        public void Create()
        {
            
            Mapper.CreateMap<Product, ProductModel>();
            Mapper.CreateMap<StandardCost, StandardCostModel>().ForMember(n => n.ProductModel, opt => opt.MapFrom(src => src.Product));
        }
    }
}