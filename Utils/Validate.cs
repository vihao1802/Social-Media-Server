using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaServer.Utils
{
    public class Validate
    {
        public static bool IsEmailValid(string email)
        {
            {
                var emailAttribute = new EmailAddressAttribute();

                return emailAttribute.IsValid(email);
            }
        }
    }
}