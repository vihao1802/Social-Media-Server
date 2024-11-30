using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaServer.ExceptionHandling
{
    public class EntityNotFoundException : Exception
    {

        public EntityNotFoundException(string message) : base(message)
        {

        }
    }
}