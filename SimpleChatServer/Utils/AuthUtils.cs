using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace SimpleChatServer.Utils
{
    public static class AuthUtils
    {
        public static string GetUserNameOrThrow(this HttpContext context)
        {
            return context.User.Identity?.Name ?? throw new InvalidOperationException();
        }

        public static long GetUserIdOrThrow(this HttpContext context)
        {
            if (long.TryParse(context.User.FindFirstValue(ClaimTypes.Sid), out long id))
                return id;

            throw new InvalidOperationException();
        }
    }
}
