using Nicole.Library.Models;
using Nicole.Library.Services;
using Nicole.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebGrease.Css.Extensions;

namespace Nicole.Web.Controllers.API
{
    public class PositionCustomerController : BaseApiController
    {
        private readonly IPositionService _positionService;
        private readonly ICustomerService _customerService;
        private readonly IPositionCustomerService _positionCustomerService;
        public PositionCustomerController(IPositionService positionService, ICustomerService customerService, IPositionCustomerService positionCustomerService)
        {
            _positionService = positionService;
            _customerService = customerService;
            _positionCustomerService = positionCustomerService;
        }
        public object Post(PositionCustomerModel model)
        {
            var postion = _positionService.GetPosition(model.PositionModel.Id);
            var customer = _customerService.GetCustomer(model.CustomerModel.Id);
            if (postion == null || customer == null || postion.IsDeleted || customer.IsDeleted)
            {
                return Failed("职位、客户不存在或者被删除");
            }
            if (_positionCustomerService.GetPositionCustomers().Any())
            {
                if (_positionCustomerService.GetPositionCustomers().Any(n => n.IsDeleted == false && n.PositionId == postion.Id && n.CustomerId == customer.Id))
                {
                    return Failed("勿重复分配");
                }
            }
            try
            {
                _positionCustomerService.Insert(new PositionCustomer
                {
                    Id = Guid.NewGuid(),
                    PositionId = postion.Id,
                    CustomerId = customer.Id
                });
                return Success();
            }
            catch (Exception ex)
            {
                return Failed(ex.Message);
            }

        }
        public object Delete(PositionCustomerModel model)
        {
            var postion = _positionService.GetPosition(model.PositionModel.Id);
            var customer = _customerService.GetCustomer(model.CustomerModel.Id);
            if (postion == null || customer == null || postion.IsDeleted || customer.IsDeleted)
            {
                return Failed("职位、客户不存在或者被删除");
            }
            try
            {
                var positionCustomers = customer.PositionCustomers.Where(n => n.PositionId == postion.Id).ToArray();
                if (positionCustomers.Any())
                {
                    positionCustomers.ForEach(n=>n.IsDeleted=true);
                    _customerService.Update();
                }
              
                return Success();
            }
            catch (Exception ex)
            {
                return Failed(ex.Message);
            }
        }

        
    }
}
