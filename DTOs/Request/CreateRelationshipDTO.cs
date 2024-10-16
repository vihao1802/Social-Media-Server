using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaServer.DTOs.Request
{
    public class CreateRelationshipDTO
    {
        [Required]
        public required Guid ReceiverId { get; set; }
    }
}