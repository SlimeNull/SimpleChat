using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleChatServer.Constants;
using SimpleChatServer.Models;
using SimpleChatServer.Models.API;
using SimpleChatServer.Models.Chat;
using SimpleChatServer.Utils;

namespace SimpleChatServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = AuthRoles.User)]
    public class PostController : ControllerBase
    {
        public PostController(
            ChatDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public ChatDbContext DbContext { get; }

        [HttpPost(nameof(SendPost))]
        public async Task<ApiResult> SendPost([FromBody] ContentModel model)
        {
            long userId = HttpContext.GetUserIdOrThrow();

            await DbContext.UserPosts.AddAsync(
                new ChatUserPost()
                {
                    UserId = userId,
                    Content = model.Content
                });

            await DbContext.SaveChangesAsync();

            return ApiResult.OkNone();
        }

        [HttpPost(nameof(GetPosts))]
        public ApiResult<List<PostModel>> GetPosts([FromBody] GetPostsModel model)
        {
            List<PostModel> list = new List<PostModel>();

            IQueryable<ChatUserPost> query =
                DbContext.UserPosts.Where(post => post.UserId == model.UserId).OrderBy(post => post.Time);

            if (model.TimeStart != null)
                query = query.Where(post => post.Time > model.TimeStart.Value);
            if (model.TimeEnd != null)
                query = query.Where(post => post.Time < model.TimeEnd.Value);
            if (model.Count > 0)
                query = query.Take(model.Count);

            foreach (var post in query)
                list.Add(new PostModel(post.Id, post.UserId, post.Content, post.Time));

            return ApiResult.Ok(list);
        }

        [HttpPost(nameof(GetLatestPosts))]
        public ApiResult<List<PostModel>> GetLatestPosts([FromBody] GetPostsModel model)
        {
            List<PostModel> list = new List<PostModel>();

            IQueryable<ChatUserPost> query =
                DbContext.UserPosts.Where(post => post.UserId == model.UserId).OrderByDescending(post => post.Time);

            if (model.TimeStart != null)
                query = query.Where(post => post.Time > model.TimeStart.Value);
            if (model.TimeEnd != null)
                query = query.Where(post => post.Time < model.TimeEnd.Value);
            if (model.Count > 0)
                query = query.Take(model.Count);

            foreach (var post in query.Reverse())
                list.Add(new PostModel(post.Id, post.UserId, post.Content, post.Time));

            return ApiResult.Ok(list);
        }
    }
}
