using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Models;

namespace SocialMediaServer.Mappers
{
    public static class MessengeMappers
    {
        public static MessengeResponseDTO ToMessengeResponseDTO(this Messenge r){
            return new MessengeResponseDTO{
                Content = r.Content,
                MediaContents = r.MediaContents,
                SenderId = r.SenderId,
                ReceiverId = r.ReceiverId,
                Sent_at = r.Sent_at,
                ReplyToId = r.ReplyToId,
            };
        }
    }
}