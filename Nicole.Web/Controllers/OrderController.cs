﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nicole.Web.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult MyOrder()
        {
            return View();
        }
    }
}