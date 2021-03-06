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
    public class SampleMapper : ISampleMapper
    {
        public void Create()
        {
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            Mapper.CreateMap<SampleReview, SampleReviewModel>();
            Mapper.CreateMap<Employee, EmployeeModel>();
            Mapper.CreateMap<Position, PositionModel>()
                .ForMember(n => n.ParentId, opt => opt.MapFrom(src => src.Parent.Id))
                .ForMember(n => n.CurrentEmployeeModel,
                    opt =>
                        opt.MapFrom(
                            src =>
                                Mapper.Map<Employee, EmployeeModel>(src.EmployeePostions.Where(
                                    e => e.StartDate <= currentDate && (e.EndDate == null || e.EndDate >= currentDate) && e.IsDeleted == false)
                                    .Select(p => p.Employee)
                                    .FirstOrDefault())
                            ));
            Mapper.CreateMap<CustomerType, CustomerTypeModel>();
            Mapper.CreateMap<Customer, CustomerModel>();
            Mapper.CreateMap<Product, ProductModel>();
            Mapper.CreateMap<Sample, SampleModel>()
                .ForMember(n => n.CustomerModel, opt => opt.MapFrom(src => src.Customer))
                .ForMember(n => n.ProductModel, opt => opt.MapFrom(src => src.Product))
                .ForMember(n => n.PositionModel, opt => opt.MapFrom(src => src.Position))
                .ForMember(n => n.State,
                    opt =>
                        opt.MapFrom(
                            src =>
                                src.IsApproved
                                    ? "已审核"
                                    : (src.SampleReviews.OrderByDescending(o => o.CreatedTime).FirstOrDefault().IsReturn
                                        ? "被退回"
                                        : src.SampleReviews.OrderByDescending(o => o.CreatedTime)
                                            .FirstOrDefault()
                                            .SendToRole.Name))).ForMember(n => n.CurrentSampleReview,
                                                opt =>
                                                    opt.MapFrom(
                                                        src =>
                                                            src.SampleReviews.OrderByDescending(p => p.CreatedTime)
                                                                .FirstOrDefault()));
        }
       
    }
}