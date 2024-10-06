using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMediaServer.DTOs;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;

namespace SocialMediaServer.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        public UserRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public Task<IdentityResult> Register(User newUser, string password)
        {
            try
            {

                var created_user = _userManager.CreateAsync(newUser, password);
                return created_user;
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}