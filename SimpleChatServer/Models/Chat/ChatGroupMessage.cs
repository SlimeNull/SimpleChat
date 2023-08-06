namespace SimpleChatServer.Models.Chat
{
    public class ChatGroupMessage
    {
        public long Id { get; set; }
        public long GroupId { get; set; }
        public long SenderId { get; set; }

        public string Message { get; set; } = string.Empty;

        public DateTime Time { get; set; } = DateTime.Now;


        public ChatGroup Group { get; set; } = null!;
        public ChatUser Sender { get; set; } = null!;
    }
}
