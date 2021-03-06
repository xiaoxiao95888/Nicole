﻿using Nicole.Library.Services;
using System.Web.Mvc;

namespace Nicole.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ILeftNavigationsService _leftNavigationsService;
        public AdminController(ILeftNavigationsService leftNavigationsService)
        {
            _leftNavigationsService = leftNavigationsService;            
        }
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult StandardCostSetting()
        {
            ViewBag.Title = _leftNavigationsService.GetLeftNavigation(HttpContext.Request.Path).Name;
            return View();
        }
        public ActionResult ProductSetting()
        {
            ViewBag.Title = _leftNavigationsService.GetLeftNavigation(HttpContext.Request.Path).Name;
            return View();
        }
        public ActionResult LeftNavigationSetting()
        {
            ViewBag.Title = _leftNavigationsService.GetLeftNavigation(HttpContext.Request.Path).Name;
            return View();
        }

        public ActionResult EnquirySetting()
        {
            ViewBag.Title = _leftNavigationsService.GetLeftNavigation(HttpContext.Request.Path).Name;
            return View();
        }
        public ActionResult CustomerManager()
        {
            ViewBag.Title = _leftNavigationsService.GetLeftNavigation(HttpContext.Request.Path).Name;
            return View();
        }

        public ActionResult OrderReview()
        {
            ViewBag.Title = _leftNavigationsService.GetLeftNavigation(HttpContext.Request.Path).Name;
            return View();
        }

        public ActionResult AllOrders()
        {
            ViewBag.Title = _leftNavigationsService.GetLeftNavigation(HttpContext.Request.Path).Name;
            return View();
        }

        
    }
}