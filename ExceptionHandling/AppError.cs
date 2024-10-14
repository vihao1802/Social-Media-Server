using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaServer.ExceptionHandling
{
    public class AppError : Exception
    {
        public int StatusCode { get; }
        public AppError(string message, int statusCode = 400) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}