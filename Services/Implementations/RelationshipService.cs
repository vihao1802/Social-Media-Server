using Microsoft.AspNetCore.Identity;
using SocialMediaServer.DTOs.Response;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.Services.Interfaces;
using SocialMediaServer.Mappers;
using Microsoft.Identity.Client;
using SocialMediaServer.Models;
using SocialMediaServer.ExceptionHandling;

namespace SocialMediaServer.Services.Implementations
{

    public class RelationshipService : IRelationshipService
    {
        private readonly IRelationshipRepository _relationshipRepository;
        private readonly IUserService _userService;
        public RelationshipService(IRelationshipRepository relationshipRepository, IUserService userService)
        {
            _relationshipRepository = relationshipRepository;
            _userService = userService;
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
                await _relationshipRepository.ChangeRelationshipType(get_existed);
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
                await _relationshipRepository.ChangeRelationshipType(get_existed);
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





    }
}