namespace SimpleChatServer.Models.Chat
{
    public class ChatGroupRequest
    {
        public long Id { get; set; }

        public long UserFromId { get; set; }
        public long GroupToId { get; set; }

        public ChatUser UserFrom { get; set; } = null!;
        public ChatGroup GroupTo { get; set; } = null!;
    }
}
