using System.Text;

namespace SimpleChatServer.Configuration
{
    public class JwtConfig
    {
        public string Secret { get; set; } = string.Empty;

        public byte[] GetSecretBytes()
        {
            return Encoding.UTF8.GetBytes(Secret ?? DefaultSecret);
        }

        public static readonly string DefaultSecret = "1145141919810";
    }
}
