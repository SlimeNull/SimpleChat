using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimpleChatServer.Configuration;
using SimpleChatServer.Constants;
using SimpleChatServer.Models;
using SimpleChatServer.Models.API;
using SimpleChatServer.Models.Chat;

namespace SimpleChatServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public AuthController(
            ChatDbContext dbContext,
            IOptionsMonitor<JwtConfig> jwtConfig,
            IOptionsMonitor<SuperAdministratorConfig> superAdminConfig)
        {
            DbContext = dbContext;
            JwtConfig = jwtConfig;
            SuperAdminConfig = superAdminConfig;
        }

        public ChatDbContext DbContext { get; }
        public IOptionsMonitor<JwtConfig> JwtConfig { get; }
        public IOptionsMonitor<SuperAdministratorConfig> SuperAdminConfig { get; }

        [HttpPost(nameof(Verify))]
        [Authorize(Roles = AuthRoles.User)]
        public object Verify()
        {
            return new object();
        }


        [HttpPost(nameof(Login))]
        public async Task<ApiResult<object>> Login([FromBody] UserNameAndPasswordModel model)
        {
            if (await DbContext.Users.FirstOrDefaultAsync(user => user.UserName == model.UserName && user.Password == model.Password) is ChatUser user)
            {
                if (!user.IsActive)
                    return ApiResult.Err(2, "用户验证尚未通过, 请联系管理员通过用户注册申请");

                if (user.IsBanned)
                    return ApiResult.Err(3, "您的账户已被封禁, 更多信息请联系管理员");

                return ApiResult<object>.Ok(
                    new
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        UserProfile = new ChatUserModel(user.Id, user.UserName, user.Nickname, user.Description, user.Avatar, user.IsAdmin),
                        Token = GenerateJwtToken(user.Id, user.UserName, user.IsAdmin, false),
                    });
            }
            else
            {
                return ApiResult.Err(1, "用户名或密码错误");
            }
        }

        [HttpPost(nameof(Register))]
        public async Task<ApiResult> Register([FromBody] UserNameAndPasswordModel model)
        {
            if (!IsValidUserName(model.UserName))
                return ApiResult.Err(1, "用户名不符合规范");

            if (!IsValidPassword(model.Password))
                return ApiResult.Err(2, "用户密码不符合规范");

            if (await DbContext.Users.AnyAsync(user => user.UserName == model.UserName))
                return ApiResult.Err(3, "用户已存在, 请换一个用户名");

            await DbContext.Users.AddAsync(
                new ChatUser()
                {
                    UserName = model.UserName,
                    Password = model.Password
                });

            await DbContext.SaveChangesAsync();

            return ApiResult.OkObj(
                new
                {
                    UserName = model.UserName,
                    Password = model.Password
                });
        }

        [HttpPost(nameof(SuperAdminLogin))]
        public ApiResult SuperAdminLogin([FromBody] UserNameAndPasswordModel model)
        {
            if (model.UserName == SuperAdminConfig.CurrentValue.UserName &&
                model.Password == SuperAdminConfig.CurrentValue.Password)
            {
                return ApiResult.OkObj(
                    new
                    {
                        Token = GenerateJwtToken(-1, "Administrator", true, true)
                    });
            }
            else
            {
                return ApiResult.Err(1, "用户名或密码错误");
            }
        }

        private bool IsValidUserName(string username)
        {
            if (username.Length < 6 || username.Length > 20)
                return false;

            if (!char.IsAsciiLetter(username[0]))
                return false;

            for (int i = 1; i < username.Length; i++)
            {
                if (!char.IsAsciiLetter(username[i]) &&
                    !char.IsDigit(username[i]) &&
                    username[i] != '_')
                    return false;
            }

            return true;
        }

        private bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            if (password.Length < 6 || password.Length > 30)
                return false;

            foreach (char c in password)
            {
                if (!char.IsLetterOrDigit(c) && "!@#$%^&*()".IndexOf(c) == -1)
                {
                    return false;
                }
            }

            return true;
        }

        private string GenerateJwtToken(long userId, string username, bool isAdmin, bool isSuperAdmin)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = JwtConfig.CurrentValue.GetSecretBytes();

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                    new []
                    {
                        new Claim(ClaimTypes.Sid, $"{userId}"),
                        new Claim(ClaimTypes.Name, username),
                        new Claim(ClaimTypes.Role, AuthRoles.User)
                    }),

                Expires = DateTime.Now.AddDays(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            if (isAdmin)
                tokenDescriptor.Subject.AddClaim(
                    new Claim(ClaimTypes.Role, AuthRoles.Administrator));

            if (isSuperAdmin)
                tokenDescriptor.Subject.AddClaim(
                    new Claim(ClaimTypes.Role, AuthRoles.SuperAdministrator));

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var tokenStr = jwtTokenHandler.WriteToken(token);

            return tokenStr;
        }
    }
}
