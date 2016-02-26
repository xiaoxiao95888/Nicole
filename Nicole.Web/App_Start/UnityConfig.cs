using System.Web.Http;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Nicole.Library.Services;
using Nicole.Service.Services;
using Nicole.Web.MapperHelper;
using Unity.Mvc5;

namespace Nicole.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = BuildUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }
        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            RegisterTypes(container);

            return container;
        }
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IAccountService, AccountService>();
            container.RegisterType<IEnquiryService, EnquiryService>();
            container.RegisterType<IStandardCostService, StandardCostService>();
            container.RegisterType<IProductService, ProductService>();
            container.RegisterType<ICustomerService, CustomerService>();
            container.RegisterType<ILeftNavigationsService, LeftNavigationService>();
            container.RegisterType<IEmployeePostionService, EmployeePostionService>();
            container.RegisterType<IEmployeesService, EmployeesService>();
            container.RegisterType<ICustomerTypeService, CustomerTypeService>();
            container.RegisterType<IPositionCustomerService, PositionCustomerService>();
            container.RegisterType<IPositionService, PositionService>();
            container.RegisterType<IOrderService, OrderService>();
            container.RegisterType<IAuditLevelService, AuditLevelService>();
            container.RegisterType<IOrderReviewService, OrderReviewService>();
            container.RegisterType<IRoleService, RoleService>();
            container.RegisterType<IPayPeriodService, PayPeriodService>();
            container.RegisterType<IFinanceService, FinanceService>();
            container.RegisterType<IFaPiaoService, FaPiaoService>();
            container.RegisterType<IApplyExpenseTypeService, ApplyExpenseTypeService>();
            container.RegisterType<IApplyExpenseService, ApplyExpenseService>();
            container.RegisterType<ISampleReviewService, SampleReviewService>();
            container.RegisterType<ISampleService, SampleService>();

            #region mapper inject
            container.RegisterType<IMapperFactory, MapperFactory>();
            #endregion

        }
    }
}