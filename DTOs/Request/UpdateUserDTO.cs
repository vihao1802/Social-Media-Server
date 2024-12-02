using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaServer.DTOs.Request
{
    public class UpdateUserDTO
    {
        [Required(ErrorMessage = "User id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email id is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime Date_of_birth { get; set; }

        [Required]
        public string Gender { get; set; }


        [Required(ErrorMessage = "Bio is required")]
        public string Bio { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }
    }
}