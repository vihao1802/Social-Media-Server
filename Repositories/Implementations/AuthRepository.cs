
using System.Security.Policy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc.Routing;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;

namespace SocialMediaServer.Repositories.Implementations
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AuthRepository(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<string> GetPasswordResetToken(User user)
        {
            var password_reset_token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return password_reset_token;
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

        public async Task<IdentityResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return IdentityResult.Success;
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

        public async Task<IdentityResult> ResetPassword(User user, string resetToken, string newPassword)
        {
            try
            {
                var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
                return result;
            }
            catch (System.Exception)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Reset password failed!" });
            }
        }

        public Task<IdentityResult> ValidatePassword(User user, string newPassword)
        {
            var validate_result = _userManager.PasswordValidators.First().ValidateAsync(_userManager, user, newPassword);
            return validate_result;
        }

        public async Task<IdentityResult> UpdatePassword(User user, string currentPassword, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return result;
        }

        public async Task<bool> CheckLockedOut(User user)
        {
            var check_result = _userManager.IsLockedOutAsync(user);
            return check_result.Result;
        }
    }
}