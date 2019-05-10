namespace document_server2.Infrastructure.Settings
{
    public class JwtSettings
    {
        public int ExpiryInMinutes { get; set; }
        public string SigningKey { get; set; }
        public string Site { get; set; }
    }
}
