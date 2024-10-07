using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaServer.DTOs.Response
{
    public class RegisterResponseDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime Date_of_birth { get; set; }
    }
}