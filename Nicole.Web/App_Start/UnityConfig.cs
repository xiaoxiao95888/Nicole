using System.Web.Http;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Nicole.Library.Services;
using Nicole.Service.Services;
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
            //StandardCostService
            container.RegisterType<IStandardCostService, StandardCostService>();
            container.RegisterType<IProductService, ProductService>();
            container.RegisterType<ILeftNavigationsService, LeftNavigationService>();
            container.RegisterType<IEmployeePostionService, EmployeePostionService>();

        }
    }
}