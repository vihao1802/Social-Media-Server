using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaServer.DTOs.Response
{
    public class RecommendationResponseDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Profile_img { get; set; }

        public int MutualFriends { get; set; }
    }
}