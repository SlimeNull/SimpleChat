using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class GroupController : ControllerBase
    {
        public ChatDbContext DbContext { get; }

        public GroupController(ChatDbContext dbContext)
        {
            DbContext = dbContext;
        }


        [HttpPost(nameof(GetGroupProfile))]
        public ApiResult<GroupProfileModel> GetGroupProfile([FromBody] GroupIdModel model)
        {
            if (DbContext.Groups.FirstOrDefault(grp => grp.Id == model.GroupId) is not ChatGroup group)
                return ApiResult.Err(1, "群聊不存在");

            return ApiResult.Ok(
                new GroupProfileModel(group.Id, group.CreatorId, group.Name, group.Description, group.Avatar));
        }

        [HttpPost(nameof(SetGroupProfile))]
        public async Task<ApiResult> SetGroupProfile([FromBody] GroupProfileModel model)
        {
            long userId =
                HttpContext.GetUserIdOrThrow();

            if (await DbContext.Users.FirstOrDefaultAsync(user => user.Id == userId) is not ChatUser user)
                return ApiResult.Err(1, "登陆失效");

            if (DbContext.Groups.FirstOrDefault(grp => grp.Id == model.Id) is not ChatGroup group)
                return ApiResult.Err(2, "群聊不存在");

            if (group.CreatorId != userId)
                return ApiResult.Err(3, "你不是群聊的创建者");

            group.Name = model.Name ?? group.Name;
            group.Description = model.Description ?? group.Description;
            group.Avatar = model.Avatar ?? group.Avatar;

            await DbContext.SaveChangesAsync();

            return ApiResult.OkNone();
        }

        [HttpPost(nameof(GetGroups))]
        public async Task<ApiResult<IList<ChatGroupModel>>> GetGroups()
        {
            long userId =
                HttpContext.GetUserIdOrThrow();

            if (await DbContext.Users.FirstOrDefaultAsync(user => user.Id == userId) is not ChatUser user)
                return ApiResult<IList<ChatGroupModel>>.Err(1, "登陆失效");

            List<ChatGroupModel> groups = new List<ChatGroupModel>();
            foreach (var addgrp in DbContext.UserAddedGroups.Where(addgrp => addgrp.UserId == userId))
            {
                await DbContext.Entry(addgrp)
                    .Reference(e => e.Group)
                    .LoadAsync();

                var group = addgrp.Group;
                groups.Add(new ChatGroupModel(group.Id, group.CreatorId, group.Name, group.Description, group.Avatar));
            }

            return ApiResult<IList<ChatGroupModel>>.Ok(groups);
        }

        [HttpPost(nameof(GetGroupMembers))]
        public async Task<ApiResult<List<ChatUserModel>>> GetGroupMembers(GroupIdModel model)
        {
            if (await DbContext.Groups.FirstOrDefaultAsync(group => group.Id == model.GroupId) is not ChatGroup group)
                return ApiResult.Err(1, "群组不存在");

            List<ChatUserModel> members = new List<ChatUserModel>();
            foreach (var addgrp in DbContext.UserAddedGroups.Where(addgrp => addgrp.GroupId == group.Id))
            {
                await DbContext.Entry(addgrp)
                    .Reference(e => e.User)
                    .LoadAsync();

                var user = addgrp.User;

                members.Add(new ChatUserModel(user.Id, user.UserName, user.Nickname, user.Description, user.Avatar, user.IsAdmin));
            }

            return ApiResult.Ok(members);
        }

        [HttpPost(nameof(CheckUserInGroup))]
        public async Task<ApiResult<bool>> CheckUserInGroup(GroupIdModel model)
        {
            long userId =
                HttpContext.GetUserIdOrThrow();

            if (await DbContext.Users.FirstOrDefaultAsync(user => user.Id == userId) is not ChatUser user)
                return ApiResult<bool>.Err(1, "登陆失效");

            return ApiResult<bool>.Ok(user.AddedGroups.Any(grp => grp.GroupId == model.GroupId));
        }

        [HttpPost(nameof(CheckUserIsGroupAdmin))]
        public async Task<ApiResult<bool>> CheckUserIsGroupAdmin(GroupIdModel model)
        {
            long userId =
                HttpContext.GetUserIdOrThrow();

            if (await DbContext.Users.FirstOrDefaultAsync(user => user.Id == userId) is not ChatUser user)
                return ApiResult<bool>.Err(1, "登陆失效");

            if (await DbContext.UserAddedGroups.FirstOrDefaultAsync(addgrp => addgrp.UserId == userId && addgrp.GroupId == model.GroupId) is not ChatUserAddedGroup addedGroup)
                return ApiResult.Ok(false);

            return ApiResult.Ok(addedGroup.IsGroupAdmin);
        }

        [HttpPost(nameof(SearchGroups))]
        public ApiResult<IList<ChatGroupModel>> SearchGroups([FromBody] NameModel model)
        {
            string name = model.Name;
            List<ChatGroupModel> groups = new List<ChatGroupModel>();

            foreach (var group in DbContext.Groups.Where(group => group.Name.Contains(name)))
                groups.Add(new ChatGroupModel(group.Id, group.CreatorId, group.Name, group.Description, group.Avatar));

            return ApiResult<IList<ChatGroupModel>>.Ok(groups);
        }

        [HttpPost(nameof(CreateGroup))]
        public async Task<ApiResult> CreateGroup([FromBody] GroupProfileModel model)
        {
            long userId =
                HttpContext.GetUserIdOrThrow();

            if (await DbContext.Users.FirstOrDefaultAsync(user => user.Id == userId) is not ChatUser user)
                return ApiResult.Err(1, "登陆失效");

            var newGroup = await DbContext.Groups.AddAsync(
                new ChatGroup()
                {
                    CreatorId = userId,
                    Name = model.Name ?? "新建群聊",
                    Description = model.Description ?? "无描述",
                    Avatar = model.Avatar ?? null,
                });

            await DbContext.SaveChangesAsync();

            await DbContext.UserAddedGroups.AddAsync(
                new ChatUserAddedGroup()
                {
                    UserId = userId,
                    GroupId = newGroup.Entity.Id,
                    IsGroupAdmin = true,
                });

            await DbContext.SaveChangesAsync();

            return ApiResult.OkNone();
        }

        [HttpPost(nameof(DeleteGroup))]
        public async Task<ApiResult> DeleteGroup([FromBody] GroupIdModel model)
        {
            long userId =
                HttpContext.GetUserIdOrThrow();

            if (await DbContext.Users.FirstOrDefaultAsync(user => user.Id == userId) is not ChatUser user)
                return ApiResult.Err(1, "登陆失效");

            if (await DbContext.Groups.FirstOrDefaultAsync(group => group.Id == model.GroupId) is not ChatGroup group)
                return ApiResult.Err(2, "群聊不存在");

            if (group.CreatorId != userId)
                return ApiResult.Err(3, "你不是群主");

            DbContext.Remove(group);

            await DbContext.SaveChangesAsync();

            return ApiResult.OkNone();
        }

        [HttpPost(nameof(LeaveGroup))]
        public async Task<ApiResult> LeaveGroup([FromBody] GroupIdModel model)
        {
            long userId =
                HttpContext.GetUserIdOrThrow();

            if (await DbContext.Users.FirstOrDefaultAsync(user => user.Id == userId) is not ChatUser user)
                return ApiResult.Err(1, "登陆失效");

            if (await DbContext.Groups.FirstOrDefaultAsync(group => group.Id == model.GroupId) is not ChatGroup group)
                return ApiResult.Err(2, "群聊不存在");

            if (await DbContext.UserAddedGroups.FirstOrDefaultAsync(addgrp => addgrp.GroupId == model.GroupId && addgrp.UserId == userId) is not ChatUserAddedGroup addGroup)
                return ApiResult.Err(3, "你不在群聊中");

            DbContext.Remove(addGroup);

            if (group.CreatorId == userId)
                DbContext.Remove(group);

            await DbContext.SaveChangesAsync();

            return ApiResult.OkNone();
        }

        [HttpPost(nameof(SetGroupAdmin))]
        public async Task<ApiResult> SetGroupAdmin(GroupMemberOptionModel model)
        {
            long userId =
                HttpContext.GetUserIdOrThrow();

            if (await DbContext.Users.FirstOrDefaultAsync(user => user.Id == userId) is not ChatUser user)
                return ApiResult.Err(1, "登陆失效");

            if (await DbContext.Groups.FirstOrDefaultAsync(group => group.Id == model.GroupId) is not ChatGroup group)
                return ApiResult.Err(2, "群聊不存在");

            if (group.CreatorId != userId)
                return ApiResult.Err(3, "你不是群主");

            if (await DbContext.UserAddedGroups.FirstOrDefaultAsync(addgrp => addgrp.UserId == model.UserId && addgrp.GroupId == model.GroupId) is not ChatUserAddedGroup addedGroup)
                return ApiResult.Err(4, "目标用户不是群成员");

            addedGroup.IsGroupAdmin = model.Enable;

            await DbContext.SaveChangesAsync();

            return ApiResult.OkNone();
        }

        [HttpPost(nameof(GetGroupMembersIsAdmin))]
        public async Task<ApiResult<List<ChatUserModel>>> GetGroupMembersIsAdmin(GroupIdModel model)
        {
            long userId =
                HttpContext.GetUserIdOrThrow();

            if (await DbContext.Users.FirstOrDefaultAsync(user => user.Id == userId) is not ChatUser user)
                return ApiResult.Err(1, "登陆失效");

            if (await DbContext.Groups.FirstOrDefaultAsync(group => group.Id == model.GroupId) is not ChatGroup group)
                return ApiResult.Err(2, "群聊不存在");

            List<ChatUserModel> members = new List<ChatUserModel>();
            foreach (var addgrp in DbContext.UserAddedGroups.Where(addgrp => addgrp.GroupId == model.GroupId && addgrp.IsGroupAdmin))
            {
                await DbContext.Entry(addgrp)
                    .Reference(e => e.User)
                    .LoadAsync();

                var _user = addgrp.User;
                members.Add(new ChatUserModel(_user.Id, _user.UserName, _user.Nickname, _user.Description, _user.Avatar, _user.IsAdmin));
            }

            return ApiResult.Ok(members);
        }

        [HttpPost(nameof(DeleteGroupMember))]
        public async Task<ApiResult> DeleteGroupMember(GroupMemberModel model)
        {
            long userId =
                HttpContext.GetUserIdOrThrow();

            if (await DbContext.Users.FirstOrDefaultAsync(user => user.Id == userId) is not ChatUser user)
                return ApiResult.Err(1, "登陆失效");

            if (await DbContext.Groups.FirstOrDefaultAsync(group => group.Id == model.GroupId) is not ChatGroup group)
                return ApiResult.Err(2, "群聊不存在");

            if (await DbContext.UserAddedGroups.FirstOrDefaultAsync(addgrp => addgrp.UserId == userId && addgrp.GroupId == group.Id) is not ChatUserAddedGroup addedGroup)
                return ApiResult.Err(3, "你不是群聊成员");

            if (!addedGroup.IsGroupAdmin)
                return ApiResult.Err(4, "你不是群聊管理员");

            if (await DbContext.UserAddedGroups.FirstOrDefaultAsync(addgrp => addgrp.UserId == model.UserId && addgrp.GroupId == model.GroupId) is not ChatUserAddedGroup targetAddedGroup)
                return ApiResult.Err(5, "目标用户不是群聊成员");

            if (targetAddedGroup.IsGroupAdmin)
                return ApiResult.Err(6, "无法踢出群聊管理员");

            if (userId == model.UserId)
                return ApiResult.Err(7, "不能踢出自己");

            DbContext.Remove(targetAddedGroup);
            await DbContext.SaveChangesAsync();

            return ApiResult.OkNone();
        }
    }
}
