using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ApplicationCore.Exception
{
	public class AppExceptionFilter : IExceptionFilter
	{
		public void OnException(ExceptionContext context)
		{
			HttpStatusCode status = HttpStatusCode.InternalServerError;
			String message = String.Empty;

			var exceptionType = context.Exception.GetType();
			if (exceptionType == typeof(UnauthorizedAccessException))
			{
				message = "Unauthorized Access  xx";
				status = HttpStatusCode.Unauthorized;
			}
			else if (exceptionType == typeof(NotImplementedException))
			{
				message = "A server error occurred.";
				status = HttpStatusCode.NotImplemented;
			}
			
			else
			{
				message = context.Exception.Message;
				status = HttpStatusCode.NotFound;
			}
			HttpResponse response = context.HttpContext.Response;
			response.StatusCode = (int)status;
			response.ContentType = "application/json";
			var err = message + " " + context.Exception.StackTrace;
			response.WriteAsync(err);
		}
	}
}
