using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleChatServer.Models.Chat
{
    public class ChatUser
    {
        public long Id { get; set; }

        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string? Avatar { get; set; }
        public string? Nickname { get; set; }
        public string? Description { get; set; }

        public bool IsAdmin { get; set; } = false;
        public bool IsActive { get; set; } = false;
        public bool IsBanned { get; set; } = false;


        private ICollection<ChatUserRelation>? _relationsFromMe;
        private ICollection<ChatUserRelation>? _relationsToMe;
        private ICollection<ChatFriendRequest>? _friendRequestsFromMe;
        private ICollection<ChatFriendRequest>? _friendRequestsToMe;
        private ICollection<ChatGroupRequest>? _groupRequestsFromMe;
        private ICollection<ChatUserAddedGroup>? _addedGroups;
        private ICollection<ChatGroup>? _createdGroups;
        private ICollection<ChatPrivateMessage>? _sendedPrivateMessages;
        private ICollection<ChatGroupMessage>? _sendedGroupMessages;


        private ICollection<ChatUserPost>? _posts;

        public ICollection<ChatUserRelation> FriendRelationsFromMe => _relationsFromMe ??= new List<ChatUserRelation>();
        public ICollection<ChatUserRelation> FriendRelationsToMe => _relationsToMe ??= new List<ChatUserRelation>();
        public ICollection<ChatFriendRequest> FriendRequestsFromMe => _friendRequestsFromMe ??= new List<ChatFriendRequest>();
        public ICollection<ChatFriendRequest> FriendRequestsToMe => _friendRequestsToMe ??= new List<ChatFriendRequest>();
        public ICollection<ChatGroupRequest> GroupRequestsFromMe => _groupRequestsFromMe ??= new List<ChatGroupRequest>();
        public ICollection<ChatUserAddedGroup> AddedGroups => _addedGroups ??= new List<ChatUserAddedGroup>();

        public ICollection<ChatGroup> CreatedGroups => _createdGroups ??= new List<ChatGroup>();
        public ICollection<ChatPrivateMessage> SendedPrivateMessages => _sendedPrivateMessages ??= new List<ChatPrivateMessage>();
        public ICollection<ChatGroupMessage> SendedGroupMessages => _sendedGroupMessages ??= new List<ChatGroupMessage>();

        public ICollection<ChatUserPost> Posts => _posts ??= new List<ChatUserPost>();
    }
}
