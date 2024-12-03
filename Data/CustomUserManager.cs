using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace SocialMediaServer.Data
{
    public class CustomUserManager<TUser>(IUserStore<TUser> store, IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<TUser> passwordHasher, IEnumerable<IUserValidator<TUser>> userValidators,
        IEnumerable<IPasswordValidator<TUser>> passwordValidators, ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<TUser>> logger) : UserManager<TUser>(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger) where TUser : class
    {
        public override async Task<IdentityResult> CreateAsync(TUser user, string password)
        {
            // Bỏ qua kiểm tra uniqueness của UserName
            return await base.CreateAsync(user, password);
        }
    }
}