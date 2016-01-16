using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nicole.Web.Infrastructure.Exceptions
{
    public class ServiceException : Exception
    {
        public int ErrorCode { get; set; }
        public ServiceException(int errorCode, string message, Exception exception)
            : base(message, exception)
        {
            ErrorCode = errorCode;
        }
    }
}