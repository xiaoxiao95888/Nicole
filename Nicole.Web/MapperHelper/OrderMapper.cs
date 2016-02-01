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
    public class OrderMapper : IOrderMapper
    {
        public void Create()
        {
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            Mapper.Reset();
            Mapper.CreateMap<Product, ProductModel>();
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
                                    e => e.StartDate <= currentDate && (e.EndDate == null || e.EndDate >= currentDate))
                                    .Select(p => p.Employee)
                                    .FirstOrDefault())
                            ));
            Mapper.CreateMap<Enquiry, EnquiryModel>()
                .ForMember(n => n.ProductModel, opt => opt.MapFrom(src => src.Product))
                .ForMember(n => n.PositionModel, opt => opt.MapFrom(src => src.Position))
                .ForMember(n => n.CustomerModel, opt => opt.MapFrom(src => src.Customer));
            Mapper.CreateMap<OrderReview, OrderReviewModel>();
            Mapper.CreateMap<Order, OrderModel>()
                .ForMember(n => n.EnquiryModel, opt => opt.MapFrom(src => src.Enquiry))
                .ForMember(n => n.State,
                    opt =>
                        opt.MapFrom(
                            src =>
                                src.IsApproved
                                    ? "已审核"
                                    : (src.OrderReviews.OrderByDescending(o => o.CreatedTime).FirstOrDefault().IsReturn
                                        ? "被退回"
                                        : "审核中")))
                .ForMember(n => n.CanEdit,
                    opt =>
                        opt.MapFrom(
                            src =>
                                !src.IsApproved &&
                                src.OrderReviews.OrderByDescending(o => o.CreatedTime).FirstOrDefault().IsReturn))
                .ForMember(n => n.CurrentOrderReview,
                    opt =>
                        opt.MapFrom(
                            src =>
                                Mapper.Map<OrderReview, OrderReviewModel>(
                                    src.OrderReviews.OrderByDescending(p => p.CreatedTime).FirstOrDefault())));

        }
    }
}