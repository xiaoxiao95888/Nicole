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
            Mapper.Reset();
            Mapper.CreateMap<Employee, EmployeeModel>();
            Mapper.CreateMap<CustomerType, CustomerTypeModel>();
            Mapper.CreateMap<Customer, CustomerModel>()
                .ForMember(n => n.CustomerTypeModel, opt => opt.MapFrom(src => src.CustomerType))
                .ForMember(n => n.EmployeeModel,
                    opt =>
                        opt.MapFrom(
                            src =>
                                src.Position.EmployeePostions.Where(
                                    p => p.StartDate <= currentDate && (p.EndDate == null || p.EndDate >= currentDate))
                                    .Select(p => p.Employee)
                                    .FirstOrDefault()))
                .ForMember(n => n.EmployeeModels,
                    opt =>
                        opt.MapFrom(
                            src =>
                                src.PositionCustomers.SelectMany(
                                    p =>
                                        p.Position.EmployeePostions.Where(
                                            ep =>
                                                ep.StartDate <= currentDate &&
                                                (ep.EndDate == null || ep.EndDate >= currentDate))
                                            .Select(rp => rp.Employee))));
        }
    }
}