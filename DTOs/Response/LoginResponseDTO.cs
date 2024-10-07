using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaServer.DTOs.Response
{
    public class LoginResponseDTO
    {

        public string Email { get; set; }
        public string Token { get; set; }
    }
}