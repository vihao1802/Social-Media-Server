
using Microsoft.AspNetCore.Identity;
using SocialMediaServer.DTOs;
using SocialMediaServer.Models;

namespace SocialMediaServer.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IdentityResult> Register(User newUser, string Password);
        Task<User?> GetUserByEmail(string email);
        Task<SignInResult> Login(User user, string password);
    }
}