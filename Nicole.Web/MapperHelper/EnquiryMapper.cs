using System;
using System.Linq;
using AutoMapper;
using Nicole.Library.Models;
using Nicole.Web.MapperHelper.IMapperInterfaces;
using Nicole.Web.Models;

namespace Nicole.Web.MapperHelper
{
    public class EnquiryMapper : IEnquiryMapper
    {
        public void Create()
        {
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            
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
        }
    }
}