using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
    public class RequestController : ControllerBase
    {
        public RequestController(
            ChatDbContext dbContext,
            EventService eventService)
        {
            DbContext = dbContext;
            EventService = eventService;
        }

        public ChatDbContext DbContext { get; }
        public EventService EventService { get; }

        [HttpPost(nameof(RequestFriend))]
        public async Task<ApiResult> RequestFriend([FromBody] UserIdModel model)
        {
            long userId =
                HttpContext.GetUserIdOrThrow();

            if (await DbContext.Users.FirstOrDefaultAsync(user => user.Id == userId) is not ChatUser from)
                return ApiResult.Err(1, "登陆失效");

            if (await DbContext.Users.FirstOrDefaultAsync(user => user.Id == model.UserId) is not ChatUser user)
                return ApiResult.Err(2, "目标用户不存在");

            if (DbContext.FriendRequests.Any(req => req.UserFromId == userId && req.UserToId == model.UserId))
                return ApiResult.Err(3, "你已经发送过好友请求了");

            await DbContext.Entry(from)
                .Collection(e => e.FriendRelationsFromMe)
                .LoadAsync();

            await DbContext.Entry(from)
                .Collection(e => e.FriendRelationsToMe)
                .LoadAsync();

            if (from.FriendRelationsFromMe.Any(rel => rel.UserToId == model.UserId) ||
                from.FriendRelationsToMe.Any(rel => rel.UserFromId == model.UserId))
                return ApiResult.Err(3, "目标用户已经是你的好友了");

            await DbContext.FriendRequests.AddAsync(
                new ChatFriendRequest()
                {
                    UserFromId = from.Id,
                    UserToId = user.Id,
                });

            await DbContext.SaveChangesAsync();

            return ApiResult.OkNone();
        }

        [HttpPost(nameof(RequestGroup))]
        public async Task<ApiResult> RequestGroup([FromBody] GroupIdModel model)
        {
            long userId =
                HttpContext.GetUserIdOrThrow();

            if (await DbContext.Users.FirstOrDefaultAsync(user => user.Id == userId) is not ChatUser from)
                return ApiResult.Err(1, "登陆失效");

            if (await DbContext.Groups.FirstOrDefaultAsync(group => group.Id == model.GroupId) is not ChatGroup group)
                return ApiResult.Err(2, "目标群聊不存在");

            if (DbContext.GroupRequests.Any(req => req.UserFromId == userId && req.GroupToId == model.GroupId))
                return ApiResult.Err(3, "你已经发送过加群请求了");

            await DbContext.Entry(from)
                .Collection(e => e.AddedGroups)
                .LoadAsync();

            if (from.AddedGroups.Any(addgrp => addgrp.GroupId == model.GroupId))
                return ApiResult.Err(4, "你已经是群成员了");

            await DbContext.GroupRequests.AddAsync(
                new ChatGroupRequest()
                {
                    UserFromId = userId,
                    GroupToId = model.GroupId
                });

            await DbContext.SaveChangesAsync();

            return ApiResult.OkNone();
        }

        [HttpPost(nameof(GetSentFriendRequests))]
        public async Task<ApiResult<List<FriendRequestModel>>> GetSentFriendRequests()
        {
            long userId =
                HttpContext.GetUserIdOrThrow();

            if (await DbContext.Users.FirstOrDefaultAsync(user => user.Id == userId) is not ChatUser from)
                return ApiResult.Err(1, "登陆状态失效");

            await DbContext.Entry(from)
                .Collection(e => e.FriendRequestsFromMe)
                .LoadAsync();

            List<FriendRequestModel> list = new List<FriendRequestModel>();
            foreach (var req in from.FriendRequestsFromMe)
                list.Add(new FriendRequestModel(req.Id, req.UserFromId, req.UserToId));

            return ApiResult.Ok(list);
        }

        [HttpPost(nameof(GetReceivedFriendRequests))]
        public async Task<ApiResult<List<FriendRequestModel>>> GetReceivedFriendRequests()
        {
            long userId =
                HttpContext.GetUserIdOrThrow();

            if (await DbContext.Users.FirstOrDefaultAsync(user => user.Id == userId) is not ChatUser from)
                return ApiResult.Err(1, "登陆状态失效");

            await DbContext.Entry(from)
                .Collection(e => e.FriendRequestsToMe)
                .LoadAsync();

            List<FriendRequestModel> list = new List<FriendRequestModel>();
            foreach (var req in from.FriendRequestsToMe)
                list.Add(new FriendRequestModel(req.Id, req.UserFromId, req.UserToId));

            return ApiResult.Ok(list);
        }

        [HttpPost(nameof(GetSentGroupRequests))]
        public async Task<ApiResult<List<GroupRequestModel>>> GetSentGroupRequests()
        {
            long userId =
                HttpContext.GetUserIdOrThrow();

            if (await DbContext.Users.FirstOrDefaultAsync(user => user.Id == userId) is not ChatUser from)
                return ApiResult.Err(1, "登陆状态失效");

            List<GroupRequestModel> list = new List<GroupRequestModel>();
            foreach (var req in from.GroupRequestsFromMe)
                list.Add(new GroupRequestModel(req.Id, req.UserFromId, req.GroupToId));

            return ApiResult.Ok(list);
        }

        [HttpPost(nameof(GetReceivedGroupRequests))]
        public async Task<ApiResult<List<GroupRequestModel>>> GetReceivedGroupRequests()
        {
            long userId =
                HttpContext.GetUserIdOrThrow();

            if (await DbContext.Users.FirstOrDefaultAsync(user => user.Id == userId) is not ChatUser from)
                return ApiResult.Err(1, "登陆状态失效");

            List<GroupRequestModel> list = new List<GroupRequestModel>();
            foreach (var group in DbContext.UserAddedGroups.Where(grp => grp.UserId == userId && grp.IsGroupAdmin))
            {
                await DbContext.Entry(group)
                    .Reference(e => e.Group)
                    .LoadAsync();

                await DbContext.Entry(group.Group)
                    .Collection(e => e.Requests)
                    .LoadAsync();

                foreach (var req in group.Group.Requests)
                    list.Add(new GroupRequestModel(req.Id, req.UserFromId, req.GroupToId));
            }

            return ApiResult.Ok(list);
        }

        [HttpPost(nameof(AcceptFriendRequest))]
        public async Task<ApiResult> AcceptFriendRequest([FromBody] FriendRequestIdModel model)
        {
            long userId =
                HttpContext.GetUserIdOrThrow();

            if (await DbContext.Users.FirstOrDefaultAsync(user => user.Id == userId) is not ChatUser user)
                return ApiResult.Err(1, "登陆状态失效");

            if (await DbContext.FriendRequests.FirstOrDefaultAsync(req => req.Id == model.FriendRequestId) is not ChatFriendRequest request)
                return ApiResult.Err(2, "好友请求不存在");

            user.FriendRelationsToMe.Add(
                new ChatUserRelation()
                {
                    UserFromId = request.UserFromId,
                    UserToId = request.UserToId
                });

            foreach (var req in DbContext.FriendRequests
                .Where(req => req.UserFromId == request.UserFromId && req.UserToId == request.UserToId ||
                              req.UserFromId == request.UserToId && req.UserToId == request.UserFromId))
            {
                DbContext.Remove(req);
            }

            await DbContext.SaveChangesAsync();

            return ApiResult.OkNone();
        }

        [HttpPost(nameof(AcceptGroupRequest))]
        public async Task<ApiResult> AcceptGroupRequest([FromBody] GroupRequestIdModel model)
        {
            long userId =
                HttpContext.GetUserIdOrThrow();

            if (await DbContext.Users.FirstOrDefaultAsync(user => user.Id == userId) is not ChatUser user)
                return ApiResult.Err(1, "登陆状态失效");

            await DbContext.Entry(user)
                .Collection(e => e.AddedGroups)
                .LoadAsync();

            bool any = false;
            var adminGroups = DbContext.UserAddedGroups
                .Where(addgrp => addgrp.UserId == userId && addgrp.IsGroupAdmin)
                .Include(e =>e.Group)
                .Select(addgrp => addgrp.Group);

            long requestUserId = 0;

            foreach (var group in adminGroups)
            {
                await DbContext.Entry(group)
                    .Collection(e => e.Requests)
                    .LoadAsync();

                if (group.Requests.FirstOrDefault(req => req.Id == model.GroupRequestId) is ChatGroupRequest request)
                {
                    DbContext.Remove(request);
                    DbContext.UserAddedGroups.Add(
                        new ChatUserAddedGroup
                        {
                            UserId = request.UserFromId,
                            GroupId = group.Id
                        });

                    EventService.OnGroupListChanged(request.UserFromId);

                    any |= true;
                    requestUserId = request.UserFromId;
                }
            }

            if (!any)
                return ApiResult.Err(2, "群请求不存在");

            await DbContext.SaveChangesAsync();

            if (any)
                EventService.OnGroupListChanged(requestUserId);

            return ApiResult.OkNone();
        }

        [HttpPost]
        public async Task<ApiResult> RejectFriendRequest([FromBody] FriendRequestIdModel model)
        {
            long userId =
                HttpContext.GetUserIdOrThrow();

            if (await DbContext.Users.FirstOrDefaultAsync(user => user.Id == userId) is not ChatUser user)
                return ApiResult.Err(1, "登陆状态失效");

            if (user.FriendRequestsToMe.FirstOrDefault(req => req.Id == model.FriendRequestId) is not ChatFriendRequest request)
                return ApiResult.Err(2, "好友请求不存在");

            foreach (var req in DbContext.FriendRequests
                .Where(req => req.UserFromId == request.UserFromId && req.UserToId == request.UserToId ||
                              req.UserFromId == request.UserToId && req.UserToId == request.UserFromId))
            {
                DbContext.Remove(req);
            }

            await DbContext.SaveChangesAsync();

            return ApiResult.OkNone();
        }
    }
}
