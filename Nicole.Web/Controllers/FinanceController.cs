using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nicole.Web.Controllers
{
    public class FinanceController : Controller
    {
        // GET: Finance
        public ActionResult AccountReceivable()
        {
            return View();
        }
    }
}