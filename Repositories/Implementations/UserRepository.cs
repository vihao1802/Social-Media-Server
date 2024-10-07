
using Microsoft.AspNetCore.Identity;

using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;

namespace SocialMediaServer.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public UserRepository(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user;
        }

        public async Task<SignInResult> Login(User user, string password)
        {
            try
            {
                var checkPassword = await _signInManager.CheckPasswordSignInAsync(user, password, false);
                return checkPassword;
            }
            catch (System.Exception)
            {
                return SignInResult.Failed;
            }
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