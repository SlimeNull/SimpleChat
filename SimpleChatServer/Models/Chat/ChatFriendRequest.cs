namespace SimpleChatServer.Models.Chat
{
    public class ChatFriendRequest
    {
        public long Id { get; set; }

        public long UserFromId { get; set; }
        public long UserToId { get; set; }

        public ChatUser UserFrom { get; set; } = null!;
        public ChatUser UserTo { get; set; } = null!;
    }
}
