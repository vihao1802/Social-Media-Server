using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Models;

namespace SocialMediaServer.Middleware
{
    public class EmailExtractMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var claims = context.User.Claims.Select(c => new { c.Type, c.Value });
                var email = claims.FirstOrDefault(c => c.Type.Contains("emailaddress") || c.Type.Contains("email")).Value;
                if (email != null)
                    context.Items["Email"] = email;
            }
            await _next(context);
        }
    }
}