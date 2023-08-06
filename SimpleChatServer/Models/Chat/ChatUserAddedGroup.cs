namespace SimpleChatServer.Models.Chat
{
    public class ChatUserAddedGroup
    {
        public long Id { get; set; }

        public long UserId { get; set; }
        public long GroupId { get; set; }

        public bool IsGroupAdmin { get; set; } = false;

        public ChatUser User { get; set; } = null!;
        public ChatGroup Group { get; set; } = null!;
    }
}
