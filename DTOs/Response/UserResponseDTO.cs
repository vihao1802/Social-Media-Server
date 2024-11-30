using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaServer.DTOs.Response
{
    public class UserResponseDTO
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime Date_of_birth { get; set; }
        public bool Gender { get; set; }

        public string Bio { get; set; }
        public string Profile_img { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Create_at { get; set; }
    }
}