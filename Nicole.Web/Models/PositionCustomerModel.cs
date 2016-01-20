using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nicole.Web.Models
{
    public class PositionCustomerModel
    {
        public PositionModel PositionModel { get; set; }
        public CustomerModel CustomerModel { get; set; }
    }
}