using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using SocialMediaServer.DTOs;
using SocialMediaServer.DTOs.Request;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;

namespace SocialMediaServer.Services.Interfaces
{
    public interface IAuthService
    {
        Task Register(RegisterDTO registerDTO);
        Task Login(LoginDTO loginDto);
        Task<IdentityResult> Logout();
        Task ForgotPassword(string email);
        Task ResetPassword(ResetPasswordDTO resetPasswordDTO);
        Task UpdatePassword(UpdatePasswordDTO updatePasswordDTO, ClaimsPrincipal principal);

        AuthenticationProperties ExternalLoginConfig(string provider, string redirectUrl);
        Task<LoginResponseDTO> HandleExternalAuthentication(ClaimsIdentity claimsIdentity);
    }
}