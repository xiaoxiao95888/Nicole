﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Nicole.Library.Models;
using Nicole.Web.MapperHelper.IMapperInterfaces;
using Nicole.Web.Models;

namespace Nicole.Web.MapperHelper
{
    public class OrderMapper : IOrderMapper
    {
        public void Create()
        {
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            Mapper.CreateMap<Employee, EmployeeModel>();
            Mapper.CreateMap<CustomerType, CustomerTypeModel>();
            Mapper.CreateMap<Customer, CustomerModel>();
            Mapper.CreateMap<Position, PositionModel>()
               .ForMember(n => n.ParentId, opt => opt.MapFrom(src => src.Parent.Id))
               .ForMember(n => n.CurrentEmployeeModel,
                   opt =>
                       opt.MapFrom(
                           src =>
                               Mapper.Map<Employee, EmployeeModel>(src.EmployeePostions.Where(
                                   e => e.StartDate <= currentDate && (e.EndDate == null || e.EndDate >= currentDate) && e.IsDeleted==false)
                                   .Select(p => p.Employee)
                                   .FirstOrDefault())
                           ));
            Mapper.CreateMap<PayPeriod, PayPeriodModel>();
            Mapper.CreateMap<OrderReview, OrderReviewModel>();
            Mapper.CreateMap<Order, OrderModel>()
                .ForMember(n => n.PositionModel, opt => opt.MapFrom(src => src.Position))
                .ForMember(n => n.CustomerModel, opt => opt.MapFrom(src => src.Customer))
                .ForMember(n => n.HasFaPiao,
                    opt => opt.MapFrom(src => src.Finances.Where(p => !p.IsDeleted).Any(p => p.HasFaPiao)))
                .ForMember(n => n.RealAmount,
                    opt => opt.MapFrom(src => src.Finances.Where(p => !p.IsDeleted).Sum(p => p.Amount)))
                .ForMember(n => n.PayPeriodModel, opt => opt.MapFrom(src => src.PayPeriod))
                .ForMember(n => n.State,
                    opt =>
                        opt.MapFrom(
                            src =>
                                src.IsApproved
                                    ? "已审核"
                                    : (src.OrderReviews.OrderByDescending(o => o.CreatedTime).FirstOrDefault().IsReturn
                                        ? "被退回"
                                        : src.OrderReviews.OrderByDescending(o => o.CreatedTime)
                                            .FirstOrDefault()
                                            .SendToRole.Name)))
                .ForMember(n => n.CurrentOrderReview,
                    opt =>
                        opt.MapFrom(
                            src =>
                                Mapper.Map<OrderReview, OrderReviewModel>(
                                    src.OrderReviews.OrderByDescending(p => p.CreatedTime).FirstOrDefault())));

        }

        public void OrderDetail()
        {
            Mapper.CreateMap<Customer, CustomerModel>();
            Mapper.CreateMap<Product, ProductModel>();
            Mapper.CreateMap<Enquiry, EnquiryModel>()
                .ForMember(n => n.ProductModel, opt => opt.MapFrom(src => src.Product))
                .ForMember(n => n.CustomerModel, opt => opt.MapFrom(src => src.Customer));
            Mapper.CreateMap<OrderDetail, OrderDetailModel>()
                .ForMember(n => n.EnquiryModel, opt => opt.MapFrom(src => src.Enquiry))
                .ForMember(n => n.TotalPrice, opt => opt.MapFrom(src => src.Qty*src.UnitPrice));
        }
    }
}