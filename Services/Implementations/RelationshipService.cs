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

        public Task<IdentityResult> BlockUser(string sender_id, string receiver_id)
        {
            throw new NotImplementedException();
        }

        public async Task FollowUser(string sender_id, string receiver_id)
        {
            if (sender_id.Equals(receiver_id))
                throw new AppError("You can't follow yourself", 400);

            // TODO: fix exception handle
            var check_user = await _userService.GetUserById(receiver_id) ?? throw new AppError("User not found", 404);

            var duplicate = await _relationshipRepository.GetRelationshipBetweenSenderAndReceiver(sender_id, receiver_id);
            if (duplicate != null)
                throw new AppError("User already followed", 400);

            var relation = new Relationship
            {
                SenderId = sender_id,
                ReceiverId = receiver_id,
                Relationship_type = RelationshipType.Follow,
                Status = RelationshipStatus.Pending
            };
            await _relationshipRepository.FollowUser(relation);

        }

        public async Task<List<FollowerResponseDTO>?> GetUserFollower(string user_id)
        {
            var check_user = await _userService.GetUserById(user_id);
            if (check_user == null)
                return null;

            var list_following = await _relationshipRepository.GetUserFollower(user_id);

            var filter = list_following.Select(r => r.ToFollowerResponseDTO()).ToList();
            return filter;
        }

        public async Task<List<FollowingResponseDTO>?> GetUserFollowing(string user_id)
        {
            var check_user = await _userService.GetUserById(user_id);
            if (check_user == null)
                return null;

            var list_following = await _relationshipRepository.GetUserFollowing(user_id);

            var filter = list_following.Select(r => r.ToFollowingResponseDTO()).ToList();
            return filter;
        }

        public Task<IdentityResult> UnBlockUser(string sender_id, string receiver_id)
        {
            throw new NotImplementedException();
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

            await _relationshipRepository.UnfollowUser(r);
        }

    }
}