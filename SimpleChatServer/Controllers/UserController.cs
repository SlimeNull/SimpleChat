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
    public class UserController : ControllerBase
    {
        public UserController(
            ChatDbContext dbContext,
            EventService eventService)
        {
            DbContext = dbContext;
            EventService = eventService;
        }

        public ChatDbContext DbContext { get; }
        public EventService EventService { get; }

        [HttpPost(nameof(GetUserProfile))]
        public async Task<ApiResult<UserProfileModel>> GetUserProfile([FromBody] UserIdModel model)
        {
            if (await DbContext.Users.FirstOrDefaultAsync(user => user.Id == model.UserId) is not ChatUser user)
                return ApiResult<UserProfileModel>.Err(1, "用户不存在");

            return ApiResult<UserProfileModel>.Ok(
                new UserProfileModel(
                    user.Id,
                    user.UserName,
                    user.Nickname,
                    user.Description,
                    user.Avatar,
                    user.IsAdmin,
                    user.IsBanned
                ));
        }

        [HttpPost(nameof(SetSelfProfile))]
        public async Task<ApiResult> SetSelfProfile([FromBody] SetUserProfileModel model)
        {
            long userId =
                HttpContext.GetUserIdOrThrow();

            if (await DbContext.Users.FirstOrDefaultAsync(user => user.Id == userId) is not ChatUser user)
                return ApiResult.Err(1, "登陆失效");

            user.Avatar = !string.IsNullOrWhiteSpace(model.Avatar) ? model.Avatar : user.Avatar;
            user.Nickname = !string.IsNullOrWhiteSpace(model.Nickname) ? model.Nickname : user.Nickname;
            user.Description = !string.IsNullOrWhiteSpace(model.Description) ? model.Description : user.Description;

            await DbContext.SaveChangesAsync();

            return ApiResult.OkNone();
        }

        [HttpPost(nameof(GetFriends))]
        public async Task<ApiResult<IList<ChatUserModel>>> GetFriends()
        {
            long userId =
                HttpContext.GetUserIdOrThrow();

            if (await DbContext.Users.FirstOrDefaultAsync(user => user.Id == userId) is not ChatUser user)
                return ApiResult<IList<ChatUserModel>>.Err(1, "登陆失效");

            List<ChatUserModel> relations = new List<ChatUserModel>();

            foreach (var relation in DbContext.UserRelations.Where(relation => relation.UserFromId == user.Id))
            {
                await DbContext.Entry(relation)
                    .Reference(e => e.UserTo)
                    .LoadAsync();

                relations.Add(new ChatUserModel(relation.UserTo.Id, relation.UserTo.UserName, relation.UserTo.Nickname, relation.UserTo.Description, relation.UserTo.Avatar, relation.UserTo.IsAdmin));
            }

            foreach (var relation in DbContext.UserRelations.Where(relation => relation.UserToId == user.Id))
            {
                await DbContext.Entry(relation)
                    .Reference(e => e.UserFrom)
                    .LoadAsync();

                relations.Add(new ChatUserModel(relation.UserFrom.Id, relation.UserFrom.UserName, relation.UserFrom.Nickname, relation.UserFrom.Description, relation.UserFrom.Avatar, relation.UserFrom.IsAdmin));
            }

            return ApiResult<IList<ChatUserModel>>.Ok(relations);
        }

        [HttpPost(nameof(CheckUserIsFriend))]
        public async Task<ApiResult<bool>> CheckUserIsFriend(UserIdModel model)
        {
            long userId =
                HttpContext.GetUserIdOrThrow();

            if (await DbContext.Users.FirstOrDefaultAsync(user => user.Id == userId) is not ChatUser user)
                return ApiResult<bool>.Err(1, "登陆失效");

            return ApiResult<bool>.Ok(
                user.FriendRelationsFromMe.Any(rel => rel.UserToId == model.UserId) ||
                user.FriendRelationsToMe.Any(rel => rel.UserFromId == model.UserId));
        }

        [HttpPost(nameof(SearchUsers))]
        public ApiResult<IList<ChatUserModel>> SearchUsers([FromBody] NameModel model)
        {
            string name = model.Name;
            List<ChatUserModel> users = new List<ChatUserModel>();

            foreach (var user in DbContext.Users.Where(user => user.UserName.Contains(name)))
                users.Add(new ChatUserModel(user.Id, user.UserName, user.Nickname, user.Description, user.Avatar, user.IsAdmin));

            foreach (var user in DbContext.Users.Where(user => user.Nickname != null && user.Nickname.Contains(name)))
                users.Add(new ChatUserModel(user.Id, user.UserName, user.Nickname, user.Description, user.Avatar, user.IsAdmin));

            return ApiResult<IList<ChatUserModel>>.Ok(users);
        }

        [HttpPost(nameof(DeleteFriend))]
        public async Task<ApiResult> DeleteFriend(UserIdModel model)
        {
            long userId =
                HttpContext.GetUserIdOrThrow();

            if (await DbContext.Users.FirstOrDefaultAsync(user => user.Id == userId) is not ChatUser user)
                return ApiResult.Err(1, "登陆失效");
            if (await DbContext.UserRelations.FirstOrDefaultAsync(rel => rel.UserFromId == userId && rel.UserToId == model.UserId || rel.UserFromId == model.UserId && rel.UserToId == userId) is not ChatUserRelation relation)
                return ApiResult.Err(2, "好友关系不存在");

            DbContext.Remove(relation);
            await DbContext.SaveChangesAsync();


            return ApiResult.OkNone();

        }
    }
}
