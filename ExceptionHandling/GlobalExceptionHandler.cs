using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SocialMediaServer.ExceptionHandling
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly IHostEnvironment _env;
        public GlobalExceptionHandler(IHostEnvironment env)
        {
            _env = env;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is AppError appError)
            {
                httpContext.Response.StatusCode = appError.StatusCode;
            }
            else if (exception is EntityNotFoundException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            }
            else
            {
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }

            var details = new ProblemDetails
            {
                Instance = httpContext.Request.Path,
                Status = httpContext.Response.StatusCode,
                Title = "An unexpected error occurred.",
                Detail = "An error occurred while processing your request.",
                Type = "https://tools.ietf.org/html/rfc7807"
            };

            if (_env.IsDevelopment())
            {
                details.Title = exception.Message;
                details.Detail = exception.ToString(); // Include stack trace and more info
                details.Extensions["traceId"] = httpContext.TraceIdentifier;
                details.Extensions["data"] = exception.Data;
            }
            else
            {
                // Log the exception or perform other actions in production
                details.Title = "An unexpected error occurred.";
                details.Detail = "An error occurred while processing your request.";
            }

            httpContext.Response.ContentType = "application/problem+json";
            await httpContext.Response.WriteAsJsonAsync(details, cancellationToken: cancellationToken);

            return true;
        }
    }
}