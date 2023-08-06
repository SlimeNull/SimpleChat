using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using SimpleChatServer.Constants;
using SimpleChatServer.Models.API;

namespace SimpleChatServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        public FileController(
            IConfiguration configuration,
            IOptions<JwtBearerOptions> jwtBearerOptions)
        {
            RootDirectory = new DirectoryInfo(configuration.GetValue<string>("FileUploading:Root") ?? "Upload");
            JwtBearerOptions = jwtBearerOptions;
        }

        public DirectoryInfo RootDirectory { get; }
        public IOptions<JwtBearerOptions> JwtBearerOptions { get; }

        [AllowAnonymous]
        [HttpGet($"{nameof(Get)}/{{filename}}")]
        public IActionResult Get([FromRoute] string filename)
        {
            string filepath = Path.Combine(RootDirectory.FullName, filename);
            if (!System.IO.File.Exists(filepath))
                return NotFound();

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filepath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            return PhysicalFile(filepath, contentType, enableRangeProcessing: true);
        }


        [HttpPost(nameof(Upload))]
        [Authorize(Roles = AuthRoles.User)]
        public async Task<ApiResult<FileUrlModel>> Upload(IFormFile file)
        {
            if (!RootDirectory.Exists)
                RootDirectory.Create();

            MemoryStream ms = new MemoryStream();
            await file.CopyToAsync(ms);

            ms.Seek(0, SeekOrigin.Begin);
            byte[] hash = SHA256.HashData(ms);

            string filename = GenerateFileName(hash, Path.GetExtension(file.FileName));
            string filepath = Path.Combine(RootDirectory.FullName, filename);

            using (FileStream fs = System.IO.File.Create(filepath))
            {
                ms.Seek(0, SeekOrigin.Begin);
                await ms.CopyToAsync(fs);
            }

            return ApiResult.Ok(new FileUrlModel($"/api/file/get/{filename}"));
        }

        private static string GenerateFileName(byte[] hash, string fileExtension)
        {
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var hashString = BitConverter.ToString(hash).Replace("-", string.Empty);
            var fileName = $"{hashString}_{timestamp}{fileExtension}";
            return fileName;
        }
    }

    public record struct FileUrlModel(string FileUrl);
}
