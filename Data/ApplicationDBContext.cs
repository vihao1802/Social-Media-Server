using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using SocialMediaServer.Models;

namespace SocialMediaServer.Data;
public class ApplicationDBContext : IdentityDbContext<User>
{
    public ApplicationDBContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<Relationship> Relationships { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<PostViewer> PostViewers { get; set; }
    public DbSet<MediaContent> MediaContents { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<CommentReaction> CommentReactions { get; set; }
    public DbSet<GroupChat> Groups { get; set; }
    public DbSet<GroupMember> GroupMembers { get; set; }
    public DbSet<GroupMessenge> GroupMessenges { get; set; }
    public DbSet<Messenge> Messenges { get; set; }
    public DbSet<MessengeMediaContent> MessengeMediaContents { get; set; }
    public DbSet<MessageReaction> MessageReactions { get; set; }
    public DbSet<Notification> Notifications { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>()
        .Property(e => e.Gender)
        .HasColumnType("varchar(50)"); // Hoặc "varchar(50)" tùy thuộc vào DBMS

        modelBuilder.Entity<User>()
        .Property(e => e.Is_external_user)
        .HasConversion<Byte>()
        .HasColumnType("TINYINT");

        modelBuilder.Entity<User>()
        .HasIndex(e => e.NormalizedUserName).IsUnique(false);

        modelBuilder.Entity<User>()
        .HasIndex(e => e.UserName).IsUnique(false);

        modelBuilder.Entity<PostViewer>()
        .Property(e => e.Liked)
        .HasConversion<Byte>()
        .HasColumnType("TINYINT");

        modelBuilder.Entity<Post>()
        .Property(e => e.Is_story)
        .HasConversion<Byte>()
        .HasColumnType("TINYINT");

        modelBuilder.Entity<Relationship>()
            .HasOne(r => r.Sender) // Each Relationship has one Sender
            .WithMany(u => u.SentRelationships) // One User can send many Relationships
            .HasForeignKey(r => r.SenderId) // SenderId is the foreign key
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete on Sender

        // Receiver relationship (One user can receive many relationships)
        modelBuilder.Entity<Relationship>()
            .HasOne(r => r.Receiver) // Each Relationship has one Receiver
            .WithMany(u => u.ReceivedRelationships) // One User can receive many Relationships
            .HasForeignKey(r => r.ReceiverId) // ReceiverId is the foreign key
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete on Receiver


        // Configuring the Post -> PostReaction relationship
        modelBuilder.Entity<PostViewer>()
            .HasOne(pr => pr.Post) // A PostReaction has one Post
            .WithMany(p => p.PostReactions) // A Post can have many PostReactions
            .HasForeignKey(pr => pr.PostId) // The foreign key is PostId
            .OnDelete(DeleteBehavior.Cascade); // Allow cascading deletes

        modelBuilder.Entity<PostViewer>()
            .HasOne(pr => pr.User) // A PostReaction has one User
            .WithMany(u => u.PostViewers) // A User can have many PostReactions
            .HasForeignKey(pr => pr.UserId) // The foreign key is UserId
            .OnDelete(DeleteBehavior.Restrict); // Allow cascading deletes

        //Configuring the Comment -> post relationship
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Post) // A Comment has one User
            .WithMany(u => u.Comments) // A User can have many Comments
            .HasForeignKey(c => c.PostId) // The foreign key is PostId
            .OnDelete(DeleteBehavior.Cascade); // Allow cascading deletes
        //Configuring the Comment -> User relationship
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.User) // A Comment has one User
            .WithMany(u => u.Comments) // A User can have many Comments
            .HasForeignKey(c => c.UserId) // The foreign key is UserId
            .OnDelete(DeleteBehavior.Restrict); // Allow cascading deletes


        // Configuring the Comment -> CommentReaction relationship
        modelBuilder.Entity<CommentReaction>()
            .HasOne(cr => cr.User) // A CommentReaction has one User
            .WithMany(u => u.CommentReactions) // A User can have many CommentReactions
            .HasForeignKey(cr => cr.UserId) // The foreign key is UserId
            .OnDelete(DeleteBehavior.Restrict); // Allow cascading deletes
        modelBuilder.Entity<CommentReaction>()
            .HasOne(cr => cr.Comment) // A CommentReaction has one Comment
            .WithMany(c => c.CommentReactions) // A Comment can have many CommentReactions
            .HasForeignKey(cr => cr.CommentId) // The foreign key is CommentId
            .OnDelete(DeleteBehavior.Cascade); // Allow cascading deletes

        // Configuring the GroupMessenge -> GroupMessenge relationship
        modelBuilder.Entity<GroupMessenge>()
            .HasOne(gm => gm.ReplyTo) // A GroupMessenge replied to one GroupMessenge
            .WithMany(g => g.Replies) // A GroupMessenge can have many GroupMessages reply to
            .HasForeignKey(gm => gm.ReplyToId) // The foreign key is GroupId
            .OnDelete(DeleteBehavior.NoAction);

        // Configuring the GroupMessenge -> User relationship
        modelBuilder.Entity<GroupMessenge>()
            .HasOne(gm => gm.Sender) // A GroupMessage has one User
            .WithMany(u => u.GroupMessenges) // A User can have many GroupMessages
            .HasForeignKey(gm => gm.SenderId) // The foreign key is UserId
            .OnDelete(DeleteBehavior.Restrict); // Allow cascading deletes

        // Configuring the Messenge -> Messenge relationship
        modelBuilder.Entity<Messenge>()
            .HasOne(m => m.ReplyTo) // A Messenge replied to one Messenge
            .WithMany(m => m.Replies) // A Messenge can have many Messages reply to
            .HasForeignKey(m => m.ReplyToId) // The foreign key is MessengeId
            .OnDelete(DeleteBehavior.NoAction);

        // Configuring the Messenge -> User relationship
        modelBuilder.Entity<Messenge>()
            .HasOne(m => m.Sender) // A Messenge has one User
            .WithMany(u => u.MessengeSent) // A User can send many Messages
            .HasForeignKey(m => m.SenderId) // The foreign key is UserId
            .OnDelete(DeleteBehavior.Restrict); // Allow cascading deletes

        // Configuring the Messenge -> User relationship
        modelBuilder.Entity<Messenge>()
            .HasOne(m => m.Receiver) // A Messenge has one User
            .WithMany(u => u.MessengeReceived) // A User can receive many Messages
            .HasForeignKey(m => m.ReceiverId) // The foreign key is UserId
            .OnDelete(DeleteBehavior.Restrict); // Allow cascading deletes

        // Configuring the MessengeMediaContent -> Messenge relationship
        modelBuilder.Entity<MessengeMediaContent>()
            .HasOne(m => m.Messenge) // A Messenge has one User
            .WithMany(u => u.MediaContents) // A User can receive many Messages
            .HasForeignKey(m => m.MessengeId) // The foreign key is UserId
            .OnDelete(DeleteBehavior.Cascade); // Allow cascading deletes

        // Cấu hình quan hệ cho MessageReaction
        modelBuilder.Entity<MessageReaction>()
            .HasOne(mr => mr.GroupMessage)
            .WithMany(gm => gm.Reactions) // GroupMessenge có nhiều Reactions
            .HasForeignKey(mr => mr.GroupMessageId)
            .OnDelete(DeleteBehavior.Cascade); // Xóa tin nhắn thì xóa luôn reactions của tin nhắn đó

        modelBuilder.Entity<MessageReaction>()
            .HasOne(mr => mr.User)
            .WithMany(u => u.Reactions) // User có nhiều Reactions
            .HasForeignKey(mr => mr.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Xóa người dùng thì xóa luôn reactions của họ
        modelBuilder.Entity<Notification>()
                .HasOne(n => n.Group) // Một thông báo liên kết với một nhóm
                .WithMany(g => g.Notifications) // Một nhóm có nhiều thông báo
                .HasForeignKey(n => n.GroupId) // Khóa ngoại
                .OnDelete(DeleteBehavior.Cascade); // Khi xóa nhóm thì xóa thông báo

        // List<IdentityRole> roles = new List<IdentityRole>
        //     {
        //         new IdentityRole
        //         {
        //             Name = "Admin",
        //             NormalizedName = "ADMIN"
        //         },
        //         new IdentityRole
        //         {
        //             Name = "User",
        //             NormalizedName = "USER"
        //         },
        //     };
        // modelBuilder.Entity<IdentityRole>().HasData(roles);
    }
}