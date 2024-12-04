using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaServer.DTOs.Request.User
{
    public class UserQueryDTO : BaseQueryDTO
    {
        [RegularExpression(@"^(eq|neq|like|in):[\w\s,]*$", ErrorMessage = "Invalid filter format")]
        public string Username { get; set; }

        [RegularExpression(@"^(eq|neq|like|in):[\w\s,]*$", ErrorMessage = "Invalid filter format")]
        public string? Email { get; set; }
    }
}