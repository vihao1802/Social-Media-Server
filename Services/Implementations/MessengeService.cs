using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using MailKit;
using Microsoft.AspNetCore.Http.HttpResults;
using NuGet.Packaging.Signing;
using SocialMediaServer.DTOs.Request;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Mappers;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Services.Interfaces;

namespace SocialMediaServer.Services.Implementations
{
    public class MessengeService: IMessengeService
    {

        private readonly IMessengeRepository _messengeRepo;
        private readonly IRelationshipRepository _relationshipRepository;
        private readonly IUserService _userService;
        private readonly IMessengeMediaContent _messengeMediaContent;
        private readonly IMessengeFileService _messengeFileService;

        public MessengeService(IMessengeRepository messengeRepo, IUserService userService, IRelationshipRepository relationshipRepository, IMessengeMediaContent messengeMediaContent, IMessengeFileService messengeFileService){
            _messengeRepo = messengeRepo;
            _userService = userService;
            _relationshipRepository = relationshipRepository;
            _messengeMediaContent = messengeMediaContent;
            _messengeFileService = messengeFileService;
        }

        public async Task<List<MessengeResponseDTO>> GetMessagesByRelationshipIdAsync(int relationshipId){
            var list_messenges = await _messengeRepo.GetMessagesByRelationshipIdAsync(relationshipId);
            var list_messenges_dto = list_messenges.Select(m => m.ToMessengeResponseDTO()).ToList();
            return list_messenges_dto;
        }

        public async Task SendMessengeAsync(string senderId, int relationshipId, int? replytoId , string? content, List<string>? filesName){

            var relationShip = await _relationshipRepository.GetRelationshipById(relationshipId);

            if(relationShip == null){
                throw new MessageNotFoundException("Relationship not found");
            }
            
            if (relationShip.Relationship_type == RelationshipType.Block)
            {
                throw new MessageNotFoundException("You cannot send messages to a blocked user");
            }

            if (relationShip.SenderId != senderId && relationShip.ReceiverId != senderId)
            {
                throw new UnauthorizedAccessException("Sender is not part of the relationship");
            }

            var receiverId = relationShip.SenderId == senderId ? relationShip.ReceiverId : relationShip.SenderId;

            string contentDB = content != null ? content : "";

            var messenge = new Messenge{
                Content = contentDB,
                SenderId = senderId,
                ReceiverId = receiverId,
                ReplyToId = replytoId,
                Sent_at = DateTime.Now,
                RelationshipId = relationShip.Id,
            };

            await _messengeRepo.SendMessengeAsync(messenge);

            if(filesName?.Count > 0 || filesName != null) {
                var messengeMediaContents = new List<MessengeMediaContent>();
                foreach(var url in filesName) {
                    var parts = url.Split('.');
                    var extension = parts[parts.Length - 1];

                    messengeMediaContents.Add(new MessengeMediaContent
                    {
                        Media_url = url, 
                        Media_type = extension, 
                        Messenge = messenge,  
                        MessengeId = messenge.id  
                    });
                }
                await _messengeMediaContent.createMessengeMediaContentAsync(messengeMediaContents);
            } 
        }

        public async Task DeleteMessengeAsync(string senderId, int messengeId){

            var messenge = await _messengeRepo.GetMessengeAsyncById(messengeId);

            // Kiểm tra xem tin nhắn có tồn tại hay không
            if (messenge == null)
            {
                throw new MessageNotFoundException("Tin nhắn không tồn tại");
            }

            // Kiểm tra xem người dùng đang đăng nhập có phải là người gửi hay không
            if (messenge.SenderId != senderId)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền xóa tin nhắn này");
            }

            // Thực hiện xóa tin nhắn
            await _messengeRepo.DeleteMessengeAsync(messenge);
        }

    }

}