using DTOs.Response;
using SocialMediaServer.DTOs.Request.MediaContent;
using SocialMediaServer.DTOs.Request.MessageReaction;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Mappers;
using SocialMediaServer.Models;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Services.Interfaces;
using SocialMediaServer.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMediaServer.Services
{
    public class MessageReactionService : IMediaContentService
    {
        private readonly IMessageReactionRepository _messageReactionRepository;
        private readonly IGroupMessengeRepository _groupMessengeRepository;
        private readonly IGroupMemberRepository _groupMemberRepository;

        public MessageReactionService(IMessageReactionRepository messageReactionRepository, IGroupMemberRepository groupMemberRepository,
        IGroupMessengeRepository groupMessengeRepository)
        {
            _messageReactionRepository = messageReactionRepository;
            _groupMemberRepository = groupMemberRepository;
            _groupMessengeRepository = groupMessengeRepository;
        }

        public async Task<PaginatedResult<MessageReaction>> GetAllReactionsAsync(MessageReactionQueryDTO queryDTO)
        {
            return await _messageReactionRepository.GetAllReactionsAsync(queryDTO);
        }

        public async Task<MessageReaction> GetByIdAsync(int id)
        {
            var reaction = await _messageReactionRepository.GetByIdAsync(id);
            if (reaction == null)
            {
                throw new AppError("MessageReaction not found", 404);
            }
            return reaction;
        }

        public async Task<MessageReactionResponseDTO> CreateAsync(MessageReactionCreateDTO messageReaction)
        {
            var mess = await _groupMessengeRepository.GetByIdAsync(messageReaction.GroupMessageId);
            if(mess == null)
                throw new AppError("Message not found", 404);
            var member = await _groupMemberRepository.GetByGroupAndUser(mess.GroupChatId,messageReaction.UserId);
            if(member == null)
                throw new AppError("Member not found", 404);
            
            var messreaction = await _messageReactionRepository.CreateAsync(new MessageReaction{
                GroupMessageId = mess.Id,
                GroupMessage = mess,
                UserId = member.UserId,
                User = member.User,
                ReactionType = messageReaction.ReactionType
            });

            return new MessageReactionResponseDTO{
                Id = messreaction.Id,
                User = member.User.UserToUserResponseDTO(),
                GroupMessageId = mess.Id,
                ReactionType = messreaction.ReactionType,
                ReactedAt = messreaction.ReactedAt
            };
        }

        public async Task<MessageReaction> UpdateAsync(int id, MessageReaction updatedReaction)
        {
            var existingReaction = await _messageReactionRepository.GetByIdAsync(id);
            if (existingReaction == null)
            {
                throw new AppError("MessageReaction not found", 404);
            }

            existingReaction.ReactionType = updatedReaction.ReactionType;
            existingReaction.ReactedAt = updatedReaction.ReactedAt;
            existingReaction.GroupMessageId = updatedReaction.GroupMessageId;
            existingReaction.UserId = updatedReaction.UserId;

            return await _messageReactionRepository.UpdateAsync(existingReaction);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _messageReactionRepository.DeleteAsync(id);
        }

        public async Task<bool> DeleteAllByGroupMessageIdAsync(int groupMessageId)
        {
            return await _messageReactionRepository.DeleteAllByGroupMessageIdAsync(groupMessageId);
        }

        public Task<PaginatedResult<MediaContentResponseDTO>> GetAllAsync(MediaContentQueryDTO mediaContentQueryDTO)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedResult<MediaContentResponseDTO>> GetAllByPostIdAsync(int postId, MediaContentQueryDTO mediaContentQueryDTO)
        {
            throw new NotImplementedException();
        }

        Task<MediaContentResponseDTO> IMediaContentService.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<MediaContentResponseDTO> CreateAsync(MediaContentCreateDTO mediaContentCreateDTO, IFormFile mediaFile)
        {
            throw new NotImplementedException();
        }

        public Task<MediaContentResponseDTO> UpdateAsync(MediaContentUpdateDTO mediaContentUpdateDTO, int id, IFormFile mediaFile)
        {
            throw new NotImplementedException();
        }

        public Task<MediaContentResponseDTO> PatchAsync(MediaContentPatchDTO mediaContentPatchDTO, int id, IFormFile? mediaFile)
        {
            throw new NotImplementedException();
        }
    }
}
