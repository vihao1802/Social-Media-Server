using SocialMediaServer.DTOs.Request;
using SocialMediaServer.Models;
using SocialMediaServer.Utils;
using Microsoft.EntityFrameworkCore;
using SocialMediaServer.Data;
using SocialMediaServer.Repositories.Interfaces;
using SocialMediaServer.DTOs.Request.GroupMember;
using SocialMediaServer.ExceptionHandling;

public class GroupMemberRepository : IGroupMemberRepository
{
    private readonly ApplicationDBContext _dbContext;

    public GroupMemberRepository(ApplicationDBContext context)
    {
        _dbContext = context;
    }

    public async Task<GroupMember> CreateAsync(GroupMember grMember)
    {
        _dbContext.GroupMembers.Add(grMember);
        await _dbContext.SaveChangesAsync();
        return grMember;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var grMember = await _dbContext.GroupMembers.FindAsync(id);
        if (grMember == null) return false;

        _dbContext.GroupMembers.Remove(grMember);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<PaginatedResult<GroupMember>> GetAllAsync(GroupMemberQueryDTO grMemberQueryDTO)
    {
        var filterParams = new Dictionary<string, object?>
            {
                {"Id", grMemberQueryDTO.Id},
                {"GroupChatId", grMemberQueryDTO.GroupId },
                {"UserId", grMemberQueryDTO.UserId },
                {"Join_at", grMemberQueryDTO.Join_at}
            };

            var grMembers = _dbContext.GroupMembers;
            foreach (var grMember in grMembers)
            {
                grMember.GroupChat = _dbContext.Groups.FirstOrDefault(p => p.Id == grMember.GroupChatId);
                grMember.User = _dbContext.Users.FirstOrDefault(u => u.Id == grMember.UserId);
            }

            var grMembersQuery = grMembers
                .ApplyIncludes(grMemberQueryDTO.Includes)
                .ApplyFilters(filterParams)
                .ApplySorting(grMemberQueryDTO.Sort)
                .ApplyPaginationAsync(grMemberQueryDTO.Page, grMemberQueryDTO.PageSize);
            
            return await grMembersQuery;
    }

    public async Task<PaginatedResult<GroupMember>> GetAllByGroupIdAsync(int groupId, GroupMemberQueryDTO grMemberQueryDTO)
    {
        var filterParams = new Dictionary<string, object?>
            {
                {"Id", grMemberQueryDTO.Id},
                {"GroupChatId", grMemberQueryDTO.GroupId },
                {"UserId", grMemberQueryDTO.UserId },
                {"Join_at", grMemberQueryDTO.Join_at}
            };

            var grMembers = _dbContext.GroupMembers
                .Where(grMember => grMember.GroupChatId == groupId);

            foreach (var grMember in grMembers)
            {
                grMember.GroupChat = _dbContext.Groups.FirstOrDefault(p => p.Id == grMember.GroupChatId);
                grMember.User = _dbContext.Users.FirstOrDefault(u => u.Id == grMember.UserId);
            }

            var grMembersQuery = grMembers
                .ApplyIncludes(grMemberQueryDTO.Includes)
                .ApplyFilters(filterParams)
                .ApplySorting(grMemberQueryDTO.Sort)
                .ApplyPaginationAsync(grMemberQueryDTO.Page, grMemberQueryDTO.PageSize);

            return await grMembersQuery;
    }

    public async Task<PaginatedResult<GroupMember>> GetByUserIdAsync(string userId, GroupMemberQueryDTO grMemberQueryDTO)
    {
         var filterParams = new Dictionary<string, object?>
            {
                {"Id", grMemberQueryDTO.Id},
                {"GroupChatId", grMemberQueryDTO.GroupId },
                {"UserId", grMemberQueryDTO.UserId },
                {"Join_at", grMemberQueryDTO.Join_at}
            };

            var grMembers = _dbContext.GroupMembers
                .Where(comment => comment.UserId == userId);

            foreach (var GrMember in grMembers)
            {
                GrMember.GroupChat = _dbContext.Groups.FirstOrDefault(p => p.Id == GrMember.GroupChatId);
                GrMember.User = _dbContext.Users.FirstOrDefault(u => u.Id == GrMember.UserId);
            }

            var grMembersQuery = grMembers
                .ApplyIncludes(grMemberQueryDTO.Includes)
                .ApplyFilters(filterParams)
                .ApplySorting(grMemberQueryDTO.Sort)
                .ApplyPaginationAsync(grMemberQueryDTO.Page, grMemberQueryDTO.PageSize);

            return await grMembersQuery;
    }

    public async Task<GroupMember> GetByIdAsync(int id)
    {
        var grMember = await _dbContext.GroupMembers.FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new AppError("Member not found", 404);
            
        return grMember;
    }

    public async Task<GroupMember> UpdateAsync(GroupMember grMember)
    {
        _dbContext.GroupMembers.Update(grMember);
        await _dbContext.SaveChangesAsync();
        return grMember;
    }

    public async Task<bool> IsMemberOfGroupAsync(int groupChatId, string userId)
    {
        return await _dbContext.GroupMembers
            .AnyAsync(grMember => grMember.GroupChatId == groupChatId && grMember.UserId == userId && !grMember.isDelete && !grMember.isLeft);
    }

    public async Task<GroupMember?> GetByGroupAndUser(int groupChatId, string userId)
    {
        var grMember = await _dbContext.GroupMembers.FirstOrDefaultAsync(p => p.GroupChatId == groupChatId && p.UserId == userId);
        return grMember;
    }
}
