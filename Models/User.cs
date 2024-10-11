
using Microsoft.AspNetCore.Identity;

namespace SocialMediaServer.Models
{
    public class User : IdentityUser
    {
        public string? Profile_img { get; set; }
        public string? Bio { get; set; }

        public DateTime? Date_of_birth { get; set; }

        public string? PhoneNumber { get; set; }
        public bool Gender { get; set; } = true;
        public DateTime Create_at { get; set; } = DateTime.Now;
        public bool Is_disabled { get; set; } = false;
        public List<Relationship> SentRelationships { get; set; } = new List<Relationship>();
        public List<Relationship> ReceivedRelationships { get; set; } = new List<Relationship>();
        public List<Post> Posts { get; set; } = new List<Post>();
        public List<PostViewer> PostViewers { get; set; } = new List<PostViewer>();
        public List<Comment> Comments { get; set; } = new List<Comment>();

        public List<CommentReaction> CommentReactions { get; set; } = new List<CommentReaction>();
        public List<GroupMember> GroupMembers { get; set; } = new List<GroupMember>();

        public List<GroupMessenge> GroupMessenges { get; set; } = new List<GroupMessenge>();

        public List<Messenge> MessengeSent { get; set; } = new List<Messenge>();
        public List<Messenge> MessengeReceived { get; set; } = new List<Messenge>();
    }
}
