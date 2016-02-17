using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nicole.Web.Models
{
    public class FaPiaoModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public Guid FinanceId { get; set; }
    }
}