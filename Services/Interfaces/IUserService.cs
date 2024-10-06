using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SocialMediaServer.DTOs;
using SocialMediaServer.Models;

namespace SocialMediaServer.Services.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> Register(UserDTO userDto);
    }
}