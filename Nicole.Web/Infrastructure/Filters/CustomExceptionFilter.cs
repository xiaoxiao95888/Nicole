using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Nicole.Library.Services;
using Nicole.Web.Models;
using ActionFilterAttribute = System.Web.Http.Filters.ActionFilterAttribute;
using System.Web.Http.Filters;
using Nicole.Web.Infrastructure.Exceptions;

namespace Nicole.Web.Infrastructure.Filters
{
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context == null || context.Exception == null)
            {
                return;
            }

            var response = new ResponseModel();
            response.Error = true;

            var serviceException = context.Exception as ServiceException;
            if (serviceException != null)
            {
                var exception = serviceException;
                response.DebugMessage = exception.Message;
                response.ErrorCode = exception.ErrorCode;

            }
            else
            {
                response.DebugMessage = context.Exception.Message;
                response.ErrorCode = 500;
            }

            //this.GetLogger().Error(context.Exception);

            var statusCode = HttpStatusCode.InternalServerError;
            if (response.ErrorCode == 2000)
            {
                statusCode = HttpStatusCode.Unauthorized;
            }

            var errorResponse = context.Request.CreateResponse(statusCode, response);//InternalServerError
            context.Response = errorResponse;

            base.OnException(context);
        }

    }
}