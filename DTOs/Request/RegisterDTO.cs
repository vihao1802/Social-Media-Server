using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaServer.Models;

namespace SocialMediaServer.DTOs
{
    public class RegisterDTO
    {
        public string? Id { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public required string UserName { get; set; }


        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public required string Email { get; set; }


        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
    ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character")]
        public required string Password { get; set; }

        public required string Gender { get; set; } = GenderOptions.Male.ToString();


        [Required(ErrorMessage = "Date of birth is required")]
        public required DateTime Date_of_birth { get; set; }

        public string? Profile_img { get; set; }
    }
}