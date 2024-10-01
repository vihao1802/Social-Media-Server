﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SocialMediaServer.Data;

#nullable disable

namespace SocialMediaServer.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    [Migration("20241001034109_changeModelBuilder4")]
    partial class changeModelBuilder4
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SocialMediaServer.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CommentId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content_gif")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CommentId");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("SocialMediaServer.Models.CommentReaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CommentId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Reaction_at")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CommentId");

                    b.HasIndex("UserId");

                    b.ToTable("CommentReaction");
                });

            modelBuilder.Entity("SocialMediaServer.Models.GroupChat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("Group_avt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Group_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("GroupChat");
                });

            modelBuilder.Entity("SocialMediaServer.Models.GroupMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("GroupChatId")
                        .HasColumnType("int");

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Join_at")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GroupChatId");

                    b.HasIndex("UserId");

                    b.ToTable("GroupMember");
                });

            modelBuilder.Entity("SocialMediaServer.Models.GroupMessenge", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("GroupChatId")
                        .HasColumnType("int");

                    b.Property<string>("Media_content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ReplyToId")
                        .HasColumnType("int");

                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Sent_at")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("GroupChatId");

                    b.HasIndex("ReplyToId");

                    b.HasIndex("SenderId");

                    b.ToTable("GroupMessenge");
                });

            modelBuilder.Entity("SocialMediaServer.Models.Login", b =>
                {
                    b.Property<int>("LoginId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LoginId"));

                    b.Property<DateTime>("Create_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("Hash_password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("Is_disabled")
                        .HasColumnType("TINYINT");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LoginId");

                    b.HasIndex("UserId");

                    b.ToTable("Logins");
                });

            modelBuilder.Entity("SocialMediaServer.Models.MediaContent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Media_Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Media_type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.ToTable("MediaContent");
                });

            modelBuilder.Entity("SocialMediaServer.Models.Messenge", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ReceiverId")
                        .HasColumnType("int");

                    b.Property<int>("ReplyToId")
                        .HasColumnType("int");

                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Sent_at")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("ReplyToId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messenge");
                });

            modelBuilder.Entity("SocialMediaServer.Models.MessengeMediaContent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Media_type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Media_url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MessengeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MessengeId");

                    b.ToTable("MessengeMediaContent");
                });

            modelBuilder.Entity("SocialMediaServer.Models.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Create_at")
                        .HasColumnType("datetime2");

                    b.Property<int>("CreatorId")
                        .HasColumnType("int");

                    b.Property<byte>("Is_story")
                        .HasColumnType("TINYINT");

                    b.Property<int>("Visibility")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Post");
                });

            modelBuilder.Entity("SocialMediaServer.Models.PostViewer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<byte>("Liked")
                        .HasColumnType("TINYINT");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("PostViewer");
                });

            modelBuilder.Entity("SocialMediaServer.Models.Relationship", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Create_at")
                        .HasColumnType("datetime2");

                    b.Property<int>("ReceiverId")
                        .HasColumnType("int");

                    b.Property<string>("Relationship_type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId");

                    b.ToTable("Relationships");
                });

            modelBuilder.Entity("SocialMediaServer.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Bio")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Create_at")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Date_of_birth")
                        .HasColumnType("datetime2");

                    b.Property<byte>("Gender")
                        .HasColumnType("TINYINT");

                    b.Property<string>("Profile_img")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SocialMediaServer.Models.Comment", b =>
                {
                    b.HasOne("SocialMediaServer.Models.Comment", "ParentComment")
                        .WithMany("Replies")
                        .HasForeignKey("CommentId");

                    b.HasOne("SocialMediaServer.Models.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SocialMediaServer.Models.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ParentComment");

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SocialMediaServer.Models.CommentReaction", b =>
                {
                    b.HasOne("SocialMediaServer.Models.Comment", "Comment")
                        .WithMany("CommentReactions")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SocialMediaServer.Models.User", "User")
                        .WithMany("CommentReactions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Comment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SocialMediaServer.Models.GroupMember", b =>
                {
                    b.HasOne("SocialMediaServer.Models.GroupChat", "GroupChat")
                        .WithMany("Members")
                        .HasForeignKey("GroupChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SocialMediaServer.Models.User", "User")
                        .WithMany("GroupMembers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GroupChat");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SocialMediaServer.Models.GroupMessenge", b =>
                {
                    b.HasOne("SocialMediaServer.Models.GroupChat", "groupChat")
                        .WithMany()
                        .HasForeignKey("GroupChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SocialMediaServer.Models.GroupMessenge", "ReplyTo")
                        .WithMany("Replies")
                        .HasForeignKey("ReplyToId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SocialMediaServer.Models.User", "Sender")
                        .WithMany("GroupMessenges")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ReplyTo");

                    b.Navigation("Sender");

                    b.Navigation("groupChat");
                });

            modelBuilder.Entity("SocialMediaServer.Models.Login", b =>
                {
                    b.HasOne("SocialMediaServer.Models.User", "User")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SocialMediaServer.Models.MediaContent", b =>
                {
                    b.HasOne("SocialMediaServer.Models.Post", "Post")
                        .WithMany("MediaContents")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");
                });

            modelBuilder.Entity("SocialMediaServer.Models.Messenge", b =>
                {
                    b.HasOne("SocialMediaServer.Models.User", "Receiver")
                        .WithMany("MessengeReceived")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SocialMediaServer.Models.Messenge", "ReplyTo")
                        .WithMany("Replies")
                        .HasForeignKey("ReplyToId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SocialMediaServer.Models.User", "Sender")
                        .WithMany("MessengeSent")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Receiver");

                    b.Navigation("ReplyTo");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("SocialMediaServer.Models.MessengeMediaContent", b =>
                {
                    b.HasOne("SocialMediaServer.Models.Messenge", "Messenge")
                        .WithMany("MediaContents")
                        .HasForeignKey("MessengeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Messenge");
                });

            modelBuilder.Entity("SocialMediaServer.Models.Post", b =>
                {
                    b.HasOne("SocialMediaServer.Models.User", "Creator")
                        .WithMany("Posts")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("SocialMediaServer.Models.PostViewer", b =>
                {
                    b.HasOne("SocialMediaServer.Models.Post", "Post")
                        .WithMany("PostReactions")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SocialMediaServer.Models.User", "User")
                        .WithMany("PostViewers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SocialMediaServer.Models.Relationship", b =>
                {
                    b.HasOne("SocialMediaServer.Models.User", "Receiver")
                        .WithMany("ReceivedRelationships")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SocialMediaServer.Models.User", "Sender")
                        .WithMany("SentRelationships")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("SocialMediaServer.Models.Comment", b =>
                {
                    b.Navigation("CommentReactions");

                    b.Navigation("Replies");
                });

            modelBuilder.Entity("SocialMediaServer.Models.GroupChat", b =>
                {
                    b.Navigation("Members");
                });

            modelBuilder.Entity("SocialMediaServer.Models.GroupMessenge", b =>
                {
                    b.Navigation("Replies");
                });

            modelBuilder.Entity("SocialMediaServer.Models.Messenge", b =>
                {
                    b.Navigation("MediaContents");

                    b.Navigation("Replies");
                });

            modelBuilder.Entity("SocialMediaServer.Models.Post", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("MediaContents");

                    b.Navigation("PostReactions");
                });

            modelBuilder.Entity("SocialMediaServer.Models.User", b =>
                {
                    b.Navigation("CommentReactions");

                    b.Navigation("Comments");

                    b.Navigation("GroupMembers");

                    b.Navigation("GroupMessenges");

                    b.Navigation("Logins");

                    b.Navigation("MessengeReceived");

                    b.Navigation("MessengeSent");

                    b.Navigation("PostViewers");

                    b.Navigation("Posts");

                    b.Navigation("ReceivedRelationships");

                    b.Navigation("SentRelationships");
                });
#pragma warning restore 612, 618
        }
    }
}
