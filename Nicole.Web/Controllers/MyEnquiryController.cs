using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nicole.Web.Controllers
{
    public class MyEnquiryController : BaseController
    {
        // GET: Enquiry
        public ActionResult Search()
        {
            return View();
        }
    }
}