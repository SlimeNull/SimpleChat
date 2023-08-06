using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleChatServer.Constants;
using SimpleChatServer.Models;
using SimpleChatServer.Models.API;
using SimpleChatServer.Models.Chat;

namespace SimpleChatServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = AuthRoles.SuperAdministrator)]
    public class ManageController : ControllerBase
    {
        public ManageController(
            ChatDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public ChatDbContext DbContext { get; }

        [HttpPost(nameof(ActiveUser))]
        public async Task<ApiResult> ActiveUser([FromBody] UserIdModel model)
        {
            if (await DbContext.Users.FirstOrDefaultAsync(user => user.Id == model.UserId) is not ChatUser user)
                return ApiResult.Err(1, "用户不存在");

            user.IsActive = true;
            await DbContext.SaveChangesAsync();

            return ApiResult.OkNone();
        }

        [HttpPost(nameof(SetUserBan))]
        public async Task<ApiResult> SetUserBan([FromBody] UserOptionModel model)
        {
            if (await DbContext.Users.FirstOrDefaultAsync(user => user.Id == model.UserId) is not ChatUser user)
                return ApiResult.Err(1, "用户不存在");
            if (user.IsBanned == model.Enable)
                return ApiResult.Err(2, $"用户封禁状态已经是 {model.Enable} 了");

            user.IsBanned = model.Enable;
            await DbContext.SaveChangesAsync();

            return ApiResult.OkNone();
        }

        [HttpPost(nameof(SetUserAdmin))]
        public async Task<ApiResult> SetUserAdmin([FromBody] UserOptionModel model)
        {
            if (await DbContext.Users.FirstOrDefaultAsync(user => user.Id == model.UserId) is not ChatUser user)
                return ApiResult.Err(1, "用户不存在");
            if (user.IsAdmin == model.Enable)
                return ApiResult.Err(2, $"用户管理员状态已经是 {model.Enable} 了");

            user.IsAdmin = model.Enable;
            await DbContext.SaveChangesAsync();

            return ApiResult.OkNone();
        }


        [HttpPost(nameof(GetUsersNeedActive))]
        public ApiResult<IList<ChatUserModel>> GetUsersNeedActive()
        {
            List<ChatUserModel> users = new List<ChatUserModel>();

            foreach (var user in DbContext.Users.Where(user => !user.IsActive))
                users.Add(new ChatUserModel(user.Id, user.UserName, user.Nickname, user.Description, user.Avatar, user.IsAdmin));

            return ApiResult<IList<ChatUserModel>>.Ok(users);
        }

        [HttpPost(nameof(GetUsersBanned))]
        public ApiResult<IList<ChatUserModel>> GetUsersBanned()
        {
            List<ChatUserModel> users = new List<ChatUserModel>();

            foreach (var user in DbContext.Users.Where(user => user.IsBanned))
                users.Add(new ChatUserModel(user.Id, user.UserName, user.Nickname, user.Description, user.Avatar, user.IsAdmin));

            return ApiResult<IList<ChatUserModel>>.Ok(users);
        }

        [HttpPost(nameof(GetUsersIsAdmin))]
        public ApiResult<IList<ChatUserModel>> GetUsersIsAdmin()
        {
            List<ChatUserModel> users = new List<ChatUserModel>();

            foreach (var user in DbContext.Users.Where(user => user.IsAdmin))
                users.Add(new ChatUserModel(user.Id, user.UserName, user.Nickname, user.Description, user.Avatar, user.IsAdmin));

            return ApiResult<IList<ChatUserModel>>.Ok(users);
        }

        [HttpPost(nameof(GetAllUsers))]
        public ApiResult<IList<ChatUserModel>> GetAllUsers()
        {
            List<ChatUserModel> users = new List<ChatUserModel>();

            foreach (var user in DbContext.Users)
                users.Add(new ChatUserModel(user.Id, user.UserName, user.Nickname, user.Description, user.Avatar, user.IsAdmin));

            return ApiResult<IList<ChatUserModel>>.Ok(users);
        }
    }
}
