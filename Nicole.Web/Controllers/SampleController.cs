using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nicole.Web.Controllers
{
    public class SampleController : Controller
    {
        // GET: SampleApply
        public ActionResult SampleApply()
        {
            return View();
        }
        public ActionResult SampleAudit()
        {
            return View();
        }
    }
}