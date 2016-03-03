using System.Linq;
using AutoMapper;
using Nicole.Library.Models;
using Nicole.Library.Services;
using Nicole.Web.Models;

namespace Nicole.Web.Controllers.API
{
    public class CustomerTypeController : BaseApiController
    {
        private readonly ICustomerTypeService _customerTypeService;
    
        public CustomerTypeController(ICustomerTypeService customerTypeService)
        {
            _customerTypeService = customerTypeService;
        }

        public object Get()
        {
            Mapper.CreateMap<CustomerType, CustomerTypeModel>();
            return _customerTypeService.GetCustomerTypes().Select(Mapper.Map<CustomerType, CustomerTypeModel>);
        }
    }
}
