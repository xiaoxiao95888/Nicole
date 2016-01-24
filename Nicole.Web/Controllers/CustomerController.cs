using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nicole.Web.Controllers
{
    public class CustomerController : BaseController
    {
        // GET: Customer
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Search()
        {
            return View();
        }
       
    }
}