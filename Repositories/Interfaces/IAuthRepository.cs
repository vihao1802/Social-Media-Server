
using Microsoft.AspNetCore.Identity;
using SocialMediaServer.DTOs;
using SocialMediaServer.Models;

namespace SocialMediaServer.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<IdentityResult> Register(User newUser, string Password);
        Task<User?> GetUserByEmail(string email);
        Task<SignInResult> Login(User user, string password);
        Task<bool> CheckLockedOut(User user);
        Task<IdentityResult> Logout();

        Task<string> GetPasswordResetToken(User email);

        Task<IdentityResult> ResetPassword(User user, string resetToken, string newPassword);

        Task<IdentityResult> ValidatePassword(User user, string newPassword);

        Task<IdentityResult> UpdatePassword(User user, string currentPassword, string newPassword);
    }
}