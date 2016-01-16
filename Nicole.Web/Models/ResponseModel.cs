using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nicole.Web.Models
{
    public class ResponseModel
    {
        public int Id { get; set; }
        public int ErrorCode { get; set; }
        public bool Error { get; set; }
        public string Message { get; set; }
        public string DebugMessage { get; set; }
    }
}