using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleChatServer.Constants;
using SimpleChatServer.Models;
using SimpleChatServer.Models.API;
using SimpleChatServer.Models.Chat;
using SimpleChatServer.Services;

namespace SimpleChatServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = AuthRoles.Administrator)]
    public class AdminController : ControllerBase
    {
        public AdminController(
            ChatDbContext dbContext,
            EventService eventService)
        {
            DbContext = dbContext;
            EventService = eventService;
        }

        public ChatDbContext DbContext { get; }
        public EventService EventService { get; }

        [HttpPost(nameof(SetUserBan))]
        public async Task<ApiResult> SetUserBan([FromBody] UserOptionModel model)
        {
            if (await DbContext.Users.FirstOrDefaultAsync(user => user.Id == model.UserId) is not ChatUser user)
                return ApiResult.Err(1, "用户不存在");
            if (user.IsAdmin)
                return ApiResult.Err(2, "作为管理员用户, 你无法操作另外一个管理员用户");

            user.IsBanned = model.Enable;
            await DbContext.SaveChangesAsync();

            return ApiResult.OkNone();
        }

        [HttpPost(nameof(DeleteGroupMessage))]
        public async Task<ApiResult> DeleteGroupMessage(MessageIdModel model)
        {
            if (await DbContext.GroupMessages.FirstOrDefaultAsync(message => message.Id == model.MessageId) is not ChatGroupMessage message)
                return ApiResult.Err(1, "消息不存在");

            DbContext.GroupMessages.Remove(message);
            await DbContext.SaveChangesAsync();

            EventService.OnGroupMessageDeleted(model.MessageId);

            return ApiResult.OkNone();
        }

        [HttpPost(nameof(DeletePrivateMessage))]
        public async Task<ApiResult> DeletePrivateMessage(MessageIdModel model)
        {
            if (await DbContext.PrivateMessages.FirstOrDefaultAsync(message => message.Id == model.MessageId) is not ChatPrivateMessage message)
                return ApiResult.Err(1, "消息不存在");

            DbContext.PrivateMessages.Remove(message);
            await DbContext.SaveChangesAsync();

            EventService.OnPrivateMessageDeleted(model.MessageId);

            return ApiResult.OkNone();
        }
    }

    public record struct MessageIdModel(long MessageId);
}
