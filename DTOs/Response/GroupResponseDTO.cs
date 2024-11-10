using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaServer.DTOs.Response
{
    public class GroupResponseDTO
    {
        public int Id {get; set;}
        public string Name {get; set;} = string.Empty;
        public string Avatar {get; set;} = string.Empty;
        public DateTime Create_date = DateTime.Now;
    }
}