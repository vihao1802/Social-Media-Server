using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaServer.DTOs.Request.GroupChat
{
    public class GroupUpdateDTO
    {
        [Required]
        public string Group_name {get; set;} = string.Empty;
        [Required]
        public string Group_avt {get; set;} = string.Empty;
        [Required]
        public DateTime? Created_at {get; set;}
        [Required]
        public string AdminId {get; set;} = string.Empty;
        [Required]
        public bool? isDelete {get; set;}
    }
}