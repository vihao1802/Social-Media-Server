using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaServer.DTOs
{
    public class UserDTO
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }

        public required string Password { get; set; }
        public bool Gender { get; set; } = true;
        public DateTime Date_of_birth { get; set; }
    }
}