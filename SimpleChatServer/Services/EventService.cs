using SimpleChatServer.Models.API;

namespace SimpleChatServer.Services
{
    public class EventService
    {
        private void OnServerEvent(EventKind eventKind, long userId, object eventData)
        {
            ServerEvent?.Invoke(this, new ServerEventArgs(eventKind, userId, eventData));
        }


        public void OnPrivateMessageSent(long userId, long friendUserId)
        {
            OnServerEvent(EventKind.PrivateMessageSent, userId, new
            {
                FriendUserId = friendUserId
            });
        }

        public void OnGroupMessageSent(long userId, long groupId)
        {
            OnServerEvent(EventKind.GroupMessageSent, userId, new
            {
                GroupId = groupId
            });
        }


        public void OnPrivateMessageReceived(long userId, long friendUserId)
        {
            OnServerEvent(EventKind.PrivateMessageReceived, userId, new
            {
                FriendUserId = friendUserId
            });
        }

        public void OnGroupMessageReceived(long userId, long groupId)
        {
            OnServerEvent(EventKind.GroupMessageReceived, userId, new
            {
                GroupId = groupId
            });
        }

        public void OnPrivateMessageDeleted(long messageId)
        {
            OnServerEvent(EventKind.PrivateMessageDeleted, -1, new
            {
                MessageId = messageId
            });
        }

        public void OnGroupMessageDeleted(long messageId)
        {
            OnServerEvent(EventKind.GroupMessageDeleted, -1, new
            {
                MessageId = messageId
            });
        }

        public void OnFriendListChanged(long userId)
        {
            OnServerEvent(EventKind.FriendListChanged, userId, new { });
        }

        public void OnGroupListChanged(long userId)
        {
            OnServerEvent(EventKind.GroupListChanged, userId, new { });
        }


        public event EventHandler<ServerEventArgs>? ServerEvent;

        public enum EventKind
        {
            PrivateMessageSent,
            GroupMessageSent,
            PrivateMessageReceived,
            GroupMessageReceived,
            PrivateMessageDeleted,
            GroupMessageDeleted,
            FriendListChanged,
            GroupListChanged,
        }

        public class ServerEventArgs : EventArgs
        {
            public ServerEventArgs(EventKind eventKind, long userId, object eventData)
            {
                EventKind = eventKind;
                UserId = userId;
                EventData = eventData;
            }

            public EventKind EventKind { get; }
            public long UserId { get; }
            public object EventData { get; }
        }
    }
}
