using AutoMapper;
using Nicole.Library.Models;
using Nicole.Library.Services;
using Nicole.Web.Infrastructure;
using Nicole.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Nicole.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IEmployeePostionService _employeePostionService;
        private readonly ILeftNavigationsService _leftNavigationsService;

        public HomeController(IEmployeePostionService employeePostionService,
            ILeftNavigationsService leftNavigationsService
           )
        {
            _employeePostionService = employeePostionService;
            _leftNavigationsService = leftNavigationsService;

        }
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        private void BindNavigation(LeftNavigationModel rootModel, List<LeftNavigation> allLeftNavigations, IEnumerable<LeftNavigation> range)
        {
            Mapper.CreateMap<LeftNavigation, LeftNavigationModel>();
            var leftNavigations = range as LeftNavigation[] ?? range.ToArray();
            var ids = leftNavigations.Select(n => n.Id).ToList();
            var subln = allLeftNavigations.Where(n => n.Parent != null && n.Parent.Id == rootModel.Id && ids.Contains(n.Id));
            rootModel.SubModels = new List<LeftNavigationModel>();
            foreach (var leftNavigation in subln)
            {
                var subModel = Mapper.Map<LeftNavigation, LeftNavigationModel>(leftNavigation);
                rootModel.SubModels.Add(subModel);
                if (allLeftNavigations.Any(n => n.Parent == leftNavigation))
                {
                    BindNavigation(subModel, allLeftNavigations, leftNavigations);
                }
            }
        }
        public ActionResult LeftNavigation()
        {
            var currentUser = System.Web.HttpContext.Current.User.Identity.GetUser();
            if (currentUser != null)
            {
                Mapper.CreateMap<LeftNavigation, LeftNavigationModel>();
                var range = this._employeePostionService.GeEmployeePostions().Where(n => n.Employee.Id == currentUser.EmployeeId && n.StartDate <= DateTime.Now && (n.EndDate >= DateTime.Now || n.EndDate == null)).SelectMany(n => n.Position.Role.LeftNavigations).ToList();
                var rootModels =
                    range.Where(n => n.Parent == null).Select(Mapper.Map<LeftNavigation, LeftNavigationModel>).ToList();

                var allLeftNavigations = _leftNavigationsService.GetLeftNavigations().ToList();
                foreach (var leftNavigationModel in rootModels)
                {
                    BindNavigation(leftNavigationModel, allLeftNavigations, range);
                }

                return View(rootModels.ToArray());
            }
            return View();
        }
        private void BindBreadCrumb(ICollection<BreadCrumbModel> models, LeftNavigation navigation, bool active = false)
        {
            models.Add(new BreadCrumbModel
            {
                Name = navigation.Name,
                Url = navigation.Url,
                Active = active
            });
            if (navigation.Parent != null)
            {
                BindBreadCrumb(models, navigation.Parent);
            }

        }
        public ActionResult BreadCrumb()
        {
            var leftNavigation = _leftNavigationsService.GetLeftNavigation(this.HttpContext.Request.Path);
            var model = new List<BreadCrumbModel>();
            if (leftNavigation != null)
            {
                BindBreadCrumb(model, leftNavigation, true);
            }
            return View(model.ToArray());
        }
    }
}