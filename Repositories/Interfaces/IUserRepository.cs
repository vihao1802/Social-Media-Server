using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMediaServer.DTOs;
using SocialMediaServer.Models;

namespace SocialMediaServer.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IdentityResult> Register(User newUser, string Password);
    }
}