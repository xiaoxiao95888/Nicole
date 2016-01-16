using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nicole.Web.Models
{
    public class BreadCrumbModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public bool Active { get; set; }
    }
}