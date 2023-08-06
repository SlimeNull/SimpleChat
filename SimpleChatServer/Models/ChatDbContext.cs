using Microsoft.EntityFrameworkCore;
using SimpleChatServer.Models.Chat;

namespace SimpleChatServer.Models
{
    public class ChatDbContext : DbContext
    {
        public ChatDbContext(DbContextOptions options) : base(options)
        {
            Users = Set<ChatUser>();
            Groups = Set<ChatGroup>();

            UserPosts = Set<ChatUserPost>();

            PrivateMessages = Set<ChatPrivateMessage>();
            GroupMessages = Set<ChatGroupMessage>();

            UserRelations = Set<ChatUserRelation>();
            UserAddedGroups = Set<ChatUserAddedGroup>();

            FriendRequests = Set<ChatFriendRequest>();
            GroupRequests = Set<ChatGroupRequest>();

            Database.EnsureCreated();
        }

        public DbSet<ChatUser> Users { get; }
        public DbSet<ChatGroup> Groups { get; }

        public DbSet<ChatUserPost> UserPosts { get; }

        public DbSet<ChatPrivateMessage> PrivateMessages { get; }
        public DbSet<ChatGroupMessage> GroupMessages { get; }

        public DbSet<ChatUserRelation> UserRelations { get; }
        public DbSet<ChatUserAddedGroup> UserAddedGroups { get; }

        public DbSet<ChatFriendRequest> FriendRequests { get; }
        public DbSet<ChatGroupRequest> GroupRequests { get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User.FromRelations  <-->  UserRelation.UserFrom
            modelBuilder.Entity<ChatUser>()
                .HasMany(e => e.FriendRelationsFromMe)
                .WithOne(e => e.UserFrom)
                .HasForeignKey(e => e.UserFromId)
                .IsRequired();

            // User.ToRelations  <-->  UserRelation.UserTo
            modelBuilder.Entity<ChatUser>()
                .HasMany(e => e.FriendRelationsToMe)
                .WithOne(e => e.UserTo)
                .HasForeignKey(e => e.UserToId)
                .IsRequired();

            // User.AddedGroups  <-->  UserAddedGroup.User
            modelBuilder.Entity<ChatUser>()
                .HasMany(e => e.AddedGroups)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .IsRequired();

            // UserRelation.Messages  <-->  PrivateMessage.Relation
            modelBuilder.Entity<ChatUserRelation>()
                .HasMany(e => e.Messages)
                .WithOne(e => e.Relation)
                .HasForeignKey(e => e.RelationId)
                .IsRequired();

            // UserAddedGroup.Group  <-->  Group.AddedUsers
            modelBuilder.Entity<ChatUserAddedGroup>()
                .HasOne(e => e.Group)
                .WithMany(e => e.AddedUsers)
                .HasForeignKey(e => e.GroupId)
                .IsRequired();

            // Group.Creator  <-->  User.CreatedGroups
            modelBuilder.Entity<ChatGroup>()
                .HasOne(e => e.Creator)
                .WithMany(e => e.CreatedGroups)
                .HasForeignKey(e => e.CreatorId)
                .IsRequired();

            // Group.Messages  <-->  GroupMessage.Group
            modelBuilder.Entity<ChatGroup>()
                .HasMany(e => e.Messages)
                .WithOne(e => e.Group)
                .HasForeignKey(e => e.GroupId)
                .IsRequired();

            // PrivateMessage.Sender  <-->  User.SendedPrivateMessages
            modelBuilder.Entity<ChatPrivateMessage>()
                .HasOne(e => e.Sender)
                .WithMany(e => e.SendedPrivateMessages)
                .HasForeignKey(e => e.SenderId)
                .IsRequired();

            // GroupMessage.Sender  <-->  User.SendedGroupMessages
            modelBuilder.Entity<ChatGroupMessage>()
                .HasOne(e => e.Sender)
                .WithMany(e => e.SendedGroupMessages)
                .HasForeignKey(e => e.SenderId)
                .IsRequired();

            modelBuilder.Entity<ChatUser>()
                .HasMany(e => e.FriendRequestsFromMe)
                .WithOne(e => e.UserFrom)
                .HasForeignKey(e => e.UserFromId)
                .IsRequired();

            modelBuilder.Entity<ChatUser>()
                .HasMany(e => e.FriendRelationsToMe)
                .WithOne(e => e.UserTo)
                .HasForeignKey(e => e.UserToId)
                .IsRequired();

            modelBuilder.Entity<ChatUser>()
                .HasMany(e => e.GroupRequestsFromMe)
                .WithOne(e => e.UserFrom)
                .HasForeignKey(e => e.UserFromId)
                .IsRequired();

            modelBuilder.Entity<ChatGroup>()
                .HasMany(e => e.Requests)
                .WithOne(e => e.GroupTo)
                .HasForeignKey(e => e.GroupToId)
                .IsRequired();

            modelBuilder.Entity<ChatUserPost>()
                .HasOne(e => e.User)
                .WithMany(e => e.Posts)
                .HasForeignKey(e => e.UserId)
                .IsRequired();
        }
    }
}
