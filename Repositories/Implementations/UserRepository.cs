using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public Task<IdentityResult> LockUser(User user)
        {
            var result = _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
            return result;
        }
        public Task<IdentityResult> UnLockUser(User user)
        {
            var result = _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(-1));
            return result;
        }

        public async Task<List<User?>> GetAllUsers()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                return users;

            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<User?>();
            }

            // return usersQueryable;
        }

        public async Task<User?> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            return user;
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return user;
        }

        public async Task<IdentityResult> UpdateUserInformation(User user)
        {
            var update_result = await _userManager.UpdateAsync(user);
            return update_result;
        }

        public async Task<User?> GetUserByClaimPrincipal(ClaimsPrincipal claimsPrincipal)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipal);
            return user;
        }

        public async Task<IList<string>> GetUsersRoles(User user)
        {
            IList<string> roles = await _userManager.GetRolesAsync(user);

            return roles;
        }
    }
}