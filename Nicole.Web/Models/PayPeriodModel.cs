using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nicole.Web.Models
{
    public class PayPeriodModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Days { get; set; }
    }
}