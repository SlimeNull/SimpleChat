namespace SimpleChatServer.Models.Chat
{
    public class ChatPrivateMessage
    {
        public long Id { get; set; }
        public long RelationId { get; set; }
        public long SenderId { get; set; }

        public string Message { get; set; } = string.Empty;

        public DateTime Time { get; set; } = DateTime.Now;



        public ChatUserRelation Relation { get; set; } = null!;
        public ChatUser Sender { get; set; } = null!;
    }
}
