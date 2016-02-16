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
    public class FinanceMapper : IFinanceMapper
    {
        public void Create()
        {
            Mapper.Reset();
            Mapper.CreateMap<Position, PositionModel>();
            Mapper.CreateMap<Finance, FinanceModel>().ForMember(n => n.PositionModel, opt => opt.MapFrom(src => src.Position));
        }

        public void OrderToFinacePage()
        {
            Mapper.Reset();
            Mapper.CreateMap<PayPeriod, PayPeriodModel>();
            Mapper.CreateMap<Order, OrderModel>()
                .ForMember(n => n.PayPeriodModel, opt => opt.MapFrom(src => src.PayPeriod));
            Mapper.CreateMap<Customer, CustomerModel>();
            Mapper.CreateMap<Order, FinancePageModel>()
                .ForMember(n => n.OrderModel, opt => opt.MapFrom(src => src))
                .ForMember(n => n.CustomerModel, opt => opt.MapFrom(src => src.Enquiry.Customer))
                .ForMember(n => n.RealAmount,
                    opt => opt.MapFrom(src => src.Finances.Where(p => p.IsDeleted == false).Sum(p => p.Amount)));
        }
    }
}