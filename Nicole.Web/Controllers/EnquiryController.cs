using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nicole.Web.Controllers
{
    public class EnquiryController : BaseController
    {
        // GET: Enquiry
        public ActionResult Index()
        {
            return View();
        }
    }
}