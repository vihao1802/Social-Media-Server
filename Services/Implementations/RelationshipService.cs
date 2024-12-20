using Microsoft.AspNetCore.Identity;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Services.Interfaces;
using SocialMediaServer.Mappers;
using Microsoft.Identity.Client;
using SocialMediaServer.Models;
using SocialMediaServer.ExceptionHandling;
using SocialMediaServer.Utils;
using SocialMediaServer.DTOs.Request;

namespace SocialMediaServer.Services.Implementations
{

    public class RelationshipService : IRelationshipService
    {
        private readonly IRelationshipRepository _relationshipRepository;
        private readonly IUserService _userService;
        private readonly IMessengeService _messengeService;

        public RelationshipService(IRelationshipRepository relationshipRepository, IUserService userService, IMessengeService messengeService)
        {
            _relationshipRepository = relationshipRepository;
            _userService = userService;
            _messengeService = messengeService;
        }

        public async Task BlockUser(string sender_id, string receiver_id)
        {
            if (sender_id.Equals(receiver_id))
                throw new AppError("Invalid user_id", 400);

            // TODO: fix exception handle
            var check_user = await _userService.GetUserById(receiver_id) ?? throw new AppError("User not found", 404);

            var get_existed = await _relationshipRepository.GetRelationshipBetweenSenderAndReceiver(sender_id, receiver_id);

            if (get_existed != null)
            {
                get_existed.Relationship_type = RelationshipType.Block;
                await _relationshipRepository.UpdateRelationship(get_existed);
            }

            var relation = new Relationship
            {
                SenderId = sender_id,
                ReceiverId = receiver_id,
                Relationship_type = RelationshipType.Block,
                Status = RelationshipStatus.Accepted
            };
            await _relationshipRepository.CreateRelationship(relation);
        }


        public async Task UnBlockUser(string sender_id, string receiver_id)
        {
            if (sender_id.Equals(receiver_id))
                throw new AppError("Invalid user_id", 400);

            var r = await _relationshipRepository.GetRelationshipBetweenSenderAndReceiver(sender_id, receiver_id);
            if (r == null)
                return;

            await _relationshipRepository.DeleteRelationship(r);
        }


        public async Task FollowUser(string sender_id, string receiver_id)
        {
            if (sender_id.Equals(receiver_id))
                throw new AppError("Invalid user_id", 400);

            // TODO: fix exception handle
            var check_user = await _userService.GetUserById(receiver_id) ?? throw new AppError("User not found", 404);

            var get_existed = await _relationshipRepository.GetRelationshipBetweenSenderAndReceiver(sender_id, receiver_id);

            if (get_existed != null)
            {
                if (get_existed.Relationship_type == RelationshipType.Follow)
                    return;

                get_existed.Relationship_type = RelationshipType.Follow;
                await _relationshipRepository.UpdateRelationship(get_existed);
            }

            var relation = new Relationship
            {
                SenderId = sender_id,
                ReceiverId = receiver_id,
                Relationship_type = RelationshipType.Follow,
                Status = RelationshipStatus.Pending
            };
            await _relationshipRepository.CreateRelationship(relation);

        }


        public async Task UnFollowUser(string sender_id, string receiver_id)
        {
            if (sender_id.Equals(receiver_id))
                throw new AppError("You can't unfollow yourself", 400);

            // TODO: fix exception handle
            var check_user = await _userService.GetUserById(receiver_id) ?? throw new AppError("User not found", 404);

            var r = await _relationshipRepository.GetRelationshipBetweenSenderAndReceiver(sender_id, receiver_id);
            if (r == null)
                return;

            await _relationshipRepository.DeleteRelationship(r);
        }


        public async Task<List<FollowingResponseDTO>?> GetUserBlockList(string user_id)
        {

            var block_list = await _relationshipRepository.GetUserBlockList(user_id);

            var filter = block_list.Select(r => r.ToFollowingResponseDTO()).ToList();
            return filter;
        }

        public async Task<List<FollowerResponseDTO>?> GetUserFollower(string user_id)
        {
            var check_user = await _userService.GetUserById(user_id) ?? throw new AppError("User not found", 404);

            var list_following = await _relationshipRepository.GetUserFollower(user_id);

            var filter = list_following.Select(r => r.ToFollowerResponseDTO()).ToList();
            return filter;
        }

        public async Task<List<FollowingResponseDTO>?> GetUserFollowing(string user_id)
        {
            var check_user = await _userService.GetUserById(user_id) ?? throw new AppError("User not found", 404);

            var list_following = await _relationshipRepository.GetUserFollowing(user_id);

            var filter = list_following.Select(r => r.ToFollowingResponseDTO()).ToList();
            return filter;
        }

        public async Task<List<PersonalMessengerResponseDTO>?> GetCurrentUserPersonalMessenger(string user_id)
        {
            var check_user = await _userService.GetUserById(user_id) ?? throw new AppError("User not found", 404);

            var list_following_follower = await _relationshipRepository.GetUser_Following_Follower(user_id);

            var list_personal_messenger = new List<PersonalMessengerResponseDTO>();

            foreach (var r in list_following_follower)
            {
                var messengerDto = new PersonalMessengerResponseDTO();
                messengerDto.relationshipId = r.Id;
                // Determine messenger
                if (r.SenderId.Equals(user_id))
                {
                    messengerDto.Messenger = r.Receiver.UserToUserResponseDTO();
                }
                else
                {
                    messengerDto.Messenger = r.Sender.UserToUserResponseDTO();
                }

                // Get the latest message
                var latest_message = await _messengeService.GetLatestMessageByRelationshipIdAsync(r.Id);

                if (latest_message != null)
                {
                    messengerDto.Latest_message = latest_message.Content;
                    messengerDto.SenderId = latest_message.SenderId;
                    messengerDto.Message_created_at = latest_message.Sent_at;
                }
                else
                {
                    messengerDto.Latest_message = string.Empty;
                    messengerDto.SenderId = string.Empty;
                    messengerDto.Message_created_at = null;
                }

                list_personal_messenger.Add(messengerDto);
            }

            return list_personal_messenger.OrderByDescending(m => m.Message_created_at).ToList();
        }

        public async Task<int> GetNumberOfFollowing(string user_id)
        {
            var check_user = await _userService.GetUserById(user_id) ?? throw new AppError("User not found", 404);

            var quantity = await _relationshipRepository.GetNumberOfFollowing(user_id);

            return quantity;
        }

        public async Task<int> GetNumberOfFollower(string user_id)
        {
            var check_user = await _userService.GetUserById(user_id) ?? throw new AppError("User not found", 404);

            var quantity = await _relationshipRepository.GetNumberOfFollower(user_id);

            return quantity;
        }

        public async Task AcceptUserFollowRequest(string sender_id, string request_id)
        {
            Relationship relationship = await _relationshipRepository.GetRelationshipBetweenSenderAndReceiver(request_id, sender_id) ?? throw new AppError("Request not found", 404);

            if (relationship.Status == RelationshipStatus.Accepted) return;

            if (relationship.Relationship_type == RelationshipType.Block) throw new AppError($"Accept failed: You have been blocked by user {request_id}", 400);

            relationship.Status = RelationshipStatus.Accepted;

            await _relationshipRepository.UpdateRelationship(relationship);
        }
        public async Task RejectUserFollowRequest(string sender_id, string request_id)
        {
            Relationship relationship = await _relationshipRepository.GetRelationshipBetweenSenderAndReceiver(request_id, sender_id) ?? throw new AppError("Request not found", 404);

            if (relationship.Status == RelationshipStatus.Accepted) throw new AppError("Request has been accepted", 400);
            if (relationship.Relationship_type == RelationshipType.Block) throw new AppError($"Accept failed: You have been blocked by user {request_id}", 400);

            await _relationshipRepository.DeleteRelationship(relationship);
        }

        public async Task<PaginatedResult<RecommendationResponseDTO>> GetRecommendation(string user_id, RecommendationQueryDTO recommendationQueryDTO)
        {
            var user = await _userService.GetUserById(user_id) ?? throw new AppError("User not found", 404);

            var recommendation = await _relationshipRepository.GetRecommendation(user_id, recommendationQueryDTO);

            return recommendation;
        }
    }
}