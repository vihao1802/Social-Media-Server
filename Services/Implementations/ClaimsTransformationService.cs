using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Mappers;
using SocialMediaServer.Repositories.Implementations;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Services.Interfaces;

namespace SocialMediaServer.Services.Implementations
{
    public class ClaimsTransformationService(IUserService userService, IUserRepository userRepository) : IClaimsTransformation
    {
        private readonly IUserService _userService = userService;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal.Identity?.IsAuthenticated != true)
            {
                throw new AppError("User is not authenticated", 401);
            }

            try
            {
                var user = await _userService.GetCurrentUser(principal);
                var roles = await _userRepository.GetUsersRoles(user.ToUserFromUserResponseDTO());

                if (roles.Count == 0)
                {
                    return principal;
                }

                foreach (var role in roles)
                {
                    if (principal.HasClaim(ClaimTypes.Role, role))
                    {
                        continue;
                    }
                    ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(ClaimTypes.Role, role));
                }
            }
            catch (Exception ex)
            { // chưa đăng ký 

            }

            return principal;
        }
    }
}