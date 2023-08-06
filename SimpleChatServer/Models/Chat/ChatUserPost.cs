namespace SimpleChatServer.Models.Chat
{
    public class ChatUserPost
    {
        public long Id { get; set; }
        public long UserId { get; set; }


        public string Content { get; set; } = string.Empty;
        public DateTime Time { get; set; } = DateTime.Now;

        public ChatUser User { get; set; } = null!;
    }
}
