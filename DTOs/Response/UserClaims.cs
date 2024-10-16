using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaServer.DTOs.Response
{
    public class UserClaims
    {
        public required string Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }

    }
}