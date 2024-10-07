using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaServer.DTOs
{
    public class RegisterDTO
    {
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

        public bool Gender { get; set; } = true;


        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime Date_of_birth { get; set; }
    }
}