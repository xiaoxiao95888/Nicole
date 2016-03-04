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
    public class CustomerMapper : ICustomerMapper
    {
        public void Create()
        {
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            
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
            Mapper.CreateMap<Customer, CustomerModel>()
                .ForMember(n => n.CustomerTypeModel, opt => opt.MapFrom(src => src.CustomerType))
                .ForMember(n => n.PositionModel,
                    opt =>
                        opt.MapFrom(
                            src => src.Position))
                .ForMember(n => n.PositionModels,
                    opt =>
                        opt.MapFrom(
                            src =>
                                src.PositionCustomers.Where(p => p.IsDeleted == false)
                                    .Select(p => p.Position)));
        }
    }
}