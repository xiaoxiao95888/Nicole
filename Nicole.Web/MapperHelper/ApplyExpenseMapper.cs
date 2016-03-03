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
    public class ApplyExpenseMapper : IApplyExpenseMapper
    {
        public void Create()
        {
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            
            Mapper.CreateMap<Employee, EmployeeModel>();
            Mapper.CreateMap<ApplyExpenseType, ApplyExpenseTypeModel>();
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
            Mapper.CreateMap<ApplyExpense, ApplyExpenseModel>()
                .ForMember(n => n.ApplyExpenseTypeModel, opt => opt.MapFrom(src => src.ApplyExpenseType))
                .ForMember(n => n.ConcernedPositionModel, opt => opt.MapFrom(src => src.ConcernedPosition))
                .ForMember(n => n.PositionModel, opt => opt.MapFrom(src => src.Position));
        }
    }
}