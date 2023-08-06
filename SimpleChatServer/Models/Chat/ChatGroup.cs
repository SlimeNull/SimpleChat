namespace SimpleChatServer.Models.Chat
{
    public class ChatGroup
    {
        public long Id { get; set; }

        public long CreatorId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string? Avatar { get; set; }


        private ICollection<ChatUserAddedGroup>? _addedUsers;
        private ICollection<ChatGroupRequest>? _groupRequests;
        private ICollection<ChatGroupMessage>? _messages;


        public ChatUser Creator { get; set; } = null!;
        public ICollection<ChatUserAddedGroup> AddedUsers => _addedUsers ??= new List<ChatUserAddedGroup>();
        public ICollection<ChatGroupRequest> Requests => _groupRequests ??= new List<ChatGroupRequest>();
        public ICollection<ChatGroupMessage> Messages => _messages ??= new List<ChatGroupMessage>();
    }
}
