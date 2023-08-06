namespace SimpleChatServer.Models.Chat
{
    public class ChatUserRelation
    {
        public long Id { get; set; }

        public long UserFromId { get; set; }
        public long UserToId { get; set; }

        public ChatUser UserFrom { get; set; } = null!;
        public ChatUser UserTo { get; set; } = null!;


        private ICollection<ChatPrivateMessage>? _messages;
        public ICollection<ChatPrivateMessage> Messages => _messages ??= new List<ChatPrivateMessage>();
    }
}
