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
        public static MessengeResponseDTO? ToMessengeResponseDTO(this Messenge r){
            if (r == null) return null;

            return new MessengeResponseDTO
            {
                Content = r.Content ?? string.Empty,
                MediaContents = r.MediaContents ?? new List<MessengeMediaContent>(),
                SenderId = r.SenderId ?? string.Empty,
                Sender = r.Sender?.UserToUserResponseDTO(),
                ReceiverId = r.ReceiverId ?? string.Empty,
                receiver = r.Receiver?.UserToUserResponseDTO(),
                Sent_at = r.Sent_at ?? DateTime.MinValue,
                ReplyToId = r.ReplyToId
            };
        }
    }
}