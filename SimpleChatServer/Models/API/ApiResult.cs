
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.OpenApi.Models;

namespace SimpleChatServer.Models.API
{

    public record struct NameModel(string Name);
    public record struct UserNameAndPasswordModel(string UserName, string Password);
    public record struct UserIdModel(long UserId);
    public record struct GroupIdModel(long GroupId);

    public record struct UserProfileModel(long Id, string UserName, string? Nickname, string? Description, string? Avatar, bool IsAdmin, bool IsBanned);
    public record struct GroupProfileModel(long Id, long CreatorId, string Name, string Description, string? Avatar);
    public record struct SetUserProfileModel(long UserId, string? Nickname, string? Description, string? Avatar);

    public record struct ChatUserModel(long Id, string UserName, string? Nickname, string? Description, string? Avatar, bool IsAdmin);
    public record struct ChatGroupModel(long Id, long CreatorId, string Name, string Description, string? Avatar);

    public record struct FriendRequestIdModel(long FriendRequestId);
    public record struct GroupRequestIdModel(long GroupRequestId);
    public record struct FriendRequestModel(long Id, long UserFromId, long UserToId);
    public record struct GroupRequestModel(long Id, long UserFromId, long GroupToId);

    public record struct ContentModel(string Content);

    public record struct GetPrivateMessagesModel(long FriendUserId, int Count, DateTime? TimeStart, DateTime? TimeEnd);
    public record struct GetGroupMessagesModel(long GroupId, int Count, DateTime? TimeStart, DateTime? TimeEnd);
    public record struct GetPostsModel(long UserId, int Count, DateTime? TimeStart, DateTime? TimeEnd);

    public record struct GroupMemberOptionModel(long GroupId, long UserId, bool Enable);
    public record struct GroupMemberModel(long GroupId, long UserId);
    public record struct UserOptionModel(long UserId, bool Enable);

    public record struct MessageModel(long Id, long SenderId, string Message, DateTime Time);
    public record struct PostModel(long Id, long UserId, string Content, DateTime Time);


    public abstract class ApiResult<T>
    {
        public abstract bool IsOk { get; }

        protected ApiResult()
        {

        }


        public static ApiOkResult<T> Ok(T value) => new ApiOkResult<T>(value);

        public static ApiErrResult<T> Err(int errorCode, string message) => new ApiErrResult<T>(errorCode, message);

        public static implicit operator ApiResult<T>(ApiErrResult err) => new ApiErrResult<T>(err.ErrorCode, err.Message);
    }

    public class ApiOkResult<T> : ApiResult<T>
    {
        public override bool IsOk => true;

        public T Value { get; }

        public ApiOkResult(T value)
        {
            Value = value;
        }
    }

    public class ApiErrResult<T> : ApiResult<T>
    {
        public override bool IsOk => false;
        public int ErrorCode { get; }
        public string Message { get; }


        public ApiErrResult(int errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }

        public static implicit operator ApiErrResult<T>(ApiErrResult err)
        {
            return ApiErrResult<T>.Err(err.ErrorCode, err.Message);
        }
    }

    public abstract class ApiResult : ApiResult<object>
    {
        public static ApiOkResult<T> Ok<T>(T value) => new ApiOkResult<T>(value);
        public static ApiOkResult OkObj(object value) => new ApiOkResult(value);
        public static new ApiErrResult Err(int errorCode, string message) => new ApiErrResult(errorCode, message);


        public static ApiOkResult OkNone() => Ok(new object());
    }

    public class ApiOkResult : ApiResult
    {
        public override bool IsOk => true;

        public object Value { get; }

        public ApiOkResult(object value)
        {
            Value = value;
        }

        public static implicit operator ApiOkResult(ApiOkResult<object> rst)
        {
            return new ApiOkResult(rst.Value);
        }
    }

    public class ApiErrResult : ApiResult
    {
        public override bool IsOk => false;
        public int ErrorCode { get; }
        public string Message { get; }


        public ApiErrResult(int errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }

        public static implicit operator ApiErrResult(ApiErrResult<object> err)
        {
            return ApiResult.Err(err.ErrorCode, err.Message);
        }
    }
}
