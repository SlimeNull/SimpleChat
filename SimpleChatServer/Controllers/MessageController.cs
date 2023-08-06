using System.Diagnostics.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleChatServer.Constants;
using SimpleChatServer.Models;
using SimpleChatServer.Models.API;
using SimpleChatServer.Models.Chat;
using SimpleChatServer.Services;
using SimpleChatServer.Utils;

namespace SimpleChatServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = AuthRoles.User)]
    public class MessageController : ControllerBase
    {
        public ChatDbContext DbContext { get; }
        public EventService EventService { get; }

        public MessageController(
            ChatDbContext dbContext,
            EventService eventService)
        {
            DbContext = dbContext;
            EventService = eventService;
        }

        public record SendPrivateMessageModel(long UserId, string Message);
        public record SendGroupMessageModel(long GroupId, string Message);

        [HttpPost(nameof(SendPrivateMessage))]
        public async Task<ApiResult> SendPrivateMessage([FromBody] SendPrivateMessageModel model)
        {
            long userId =
                HttpContext.GetUserIdOrThrow();

            if (await DbContext.Users.FirstOrDefaultAsync(user => user.Id == userId) is not ChatUser sender)
                return ApiResult.Err(1, "登陆失效");

            ChatUserRelation? relation = await DbContext.UserRelations
                .FirstOrDefaultAsync(relation => relation.UserFromId == userId && relation.UserToId == model.UserId || relation.UserFromId == model.UserId && relation.UserToId == userId);

            if (relation == null)
                return ApiResult.Err(2, "对方不是你的好友");

            await DbContext.PrivateMessages.AddAsync(
                new ChatPrivateMessage()
                {
                    RelationId = relation.Id,
                    SenderId = sender.Id,
                    Message = model.Message
                });

            await DbContext.SaveChangesAsync();

            EventService.OnPrivateMessageSent(userId, model.UserId);
            EventService.OnPrivateMessageReceived(model.UserId, userId);

            return ApiResult.OkNone();
        }

        [HttpPost(nameof(SendGroupMessage))]
        public async Task<ApiResult> SendGroupMessage([FromBody] SendGroupMessageModel model)
        {
            long userId =
                HttpContext.GetUserIdOrThrow();

            if (await DbContext.Users.FirstOrDefaultAsync(user => user.Id == userId) is not ChatUser sender)
                return ApiResult.Err(1, "登陆失效");

            if (await DbContext.Groups.FirstOrDefaultAsync(group => group.Id == model.GroupId) is not ChatGroup group)
                return ApiResult.Err(2, "群组不存在");

            await DbContext.Entry(group)
                .Collection(e => e.AddedUsers)
                .LoadAsync();

            if (!group.AddedUsers.Any(addusr => addusr.UserId == userId))
                return ApiResult.Err(3, "你不是群组成员");

            await DbContext.GroupMessages.AddAsync(
                new ChatGroupMessage()
                {
                    GroupId = model.GroupId,
                    SenderId = sender.Id,
                    Message = model.Message
                });

            await DbContext.SaveChangesAsync();

            EventService.OnGroupMessageSent(userId, group.Id);
            foreach (var addusr in group.AddedUsers.Where(addusr => addusr.UserId != userId))
                EventService.OnGroupMessageReceived(addusr.UserId, group.Id);

            return ApiResult.OkNone();
        }

        [HttpPost(nameof(GetPrivateMessages))]
        public async Task<ApiResult<List<MessageModel>>> GetPrivateMessages(GetPrivateMessagesModel model)
        {
            long userId =
                HttpContext.GetUserIdOrThrow();

            var relation =
                DbContext.UserRelations.FirstOrDefault(rel => rel.UserFromId == userId && rel.UserToId == model.FriendUserId || rel.UserFromId == model.FriendUserId && rel.UserToId == userId);

            if (relation == null)
                return ApiResult.Err(1, "对方不是你的好友");

            await DbContext.Entry(relation)
                .Collection(e => e.Messages)
                .LoadAsync();

            List<MessageModel> messages = new List<MessageModel>();
            IEnumerable<ChatPrivateMessage> query = relation.Messages.OrderBy(msg => msg.Time);

            if (model.TimeStart != null)
                query = query.Where(message => message.Time > model.TimeStart.Value);
            if (model.TimeEnd != null)
                query = query.Where(message => message.Time < model.TimeEnd.Value);
            if (model.Count > 0)
                query = query.Take(model.Count);

            foreach (var message in query)
                messages.Add(new MessageModel(message.Id, message.SenderId, message.Message, message.Time));

            return ApiResult.Ok(messages);
        }

        [HttpPost(nameof(GetGroupMessages))]
        public async Task<ApiResult<List<MessageModel>>> GetGroupMessages(GetGroupMessagesModel model)
        {
            long userId =
                HttpContext.GetUserIdOrThrow();

            var addedGroup =
                DbContext.UserAddedGroups.FirstOrDefault(grp => grp.UserId == userId && grp.GroupId == model.GroupId);

            if (addedGroup == null)
                return ApiResult.Err(1, "你不是群成员");

            await DbContext.Entry(addedGroup)
                .Reference(e => e.Group)
                .LoadAsync();

            ChatGroup group = addedGroup.Group;

            await DbContext.Entry(group)
                .Collection(e => e.Messages)
                .LoadAsync();

            List<MessageModel> messages = new List<MessageModel>();
            IEnumerable<ChatGroupMessage> query = group.Messages.OrderBy(msg => msg.Time);

            if (model.TimeStart != null)
                query = query.Where(message => message.Time > model.TimeStart.Value);
            if (model.TimeEnd != null)
                query = query.Where(message => message.Time < model.TimeEnd.Value);
            if (model.Count > 0)
                query = query.Take(model.Count);

            foreach (var message in query)
                messages.Add(new MessageModel(message.Id, message.SenderId, message.Message, message.Time));

            return ApiResult.Ok(messages);
        }

        [HttpPost(nameof(GetLatestPrivateMessages))]
        public async Task<ApiResult<List<MessageModel>>> GetLatestPrivateMessages(GetPrivateMessagesModel model)
        {
            long userId =
                HttpContext.GetUserIdOrThrow();

            var relation =
                DbContext.UserRelations.FirstOrDefault(rel => rel.UserFromId == userId && rel.UserToId == model.FriendUserId || rel.UserFromId == model.FriendUserId && rel.UserToId == userId);

            if (relation == null)
                return ApiResult.Err(1, "对方不是你的好友");

            await DbContext.Entry(relation)
                .Collection(e => e.Messages)
                .LoadAsync();

            List<MessageModel> messages = new List<MessageModel>();
            IEnumerable<ChatPrivateMessage> query = relation.Messages.OrderByDescending(msg => msg.Time);

            if (model.TimeStart != null)
                query = query.Where(message => message.Time > model.TimeStart.Value);
            if (model.TimeEnd != null)
                query = query.Where(message => message.Time < model.TimeEnd.Value);
            if (model.Count > 0)
                query = query.Take(model.Count);

            foreach (var message in query.Reverse())
                messages.Add(new MessageModel(message.Id, message.SenderId, message.Message, message.Time));

            return ApiResult.Ok(messages);
        }

        [HttpPost(nameof(GetLatestGroupMessages))]
        public async Task<ApiResult<List<MessageModel>>> GetLatestGroupMessages(GetGroupMessagesModel model)
        {
            long userId =
                HttpContext.GetUserIdOrThrow();

            var addedGroup =
                DbContext.UserAddedGroups.FirstOrDefault(grp => grp.UserId == userId && grp.GroupId == model.GroupId);

            if (addedGroup == null)
                return ApiResult.Err(1, "你不是群成员");

            await DbContext.Entry(addedGroup)
                .Reference(e => e.Group)
                .LoadAsync();

            ChatGroup group = addedGroup.Group;

            await DbContext.Entry(group)
                .Collection(e => e.Messages)
                .LoadAsync();

            List<MessageModel> messages = new List<MessageModel>();
            IEnumerable<ChatGroupMessage> query = group.Messages.OrderByDescending(msg => msg.Time);

            if (model.TimeStart != null)
                query = query.Where(message => message.Time > model.TimeStart.Value);
            if (model.TimeEnd != null)
                query = query.Where(message => message.Time < model.TimeEnd.Value);
            if (model.Count > 0)
                query = query.Take(model.Count);

            foreach (var message in query.Reverse())
                messages.Add(new MessageModel(message.Id, message.SenderId, message.Message, message.Time));

            return ApiResult.Ok(messages);
        }

    }
}
