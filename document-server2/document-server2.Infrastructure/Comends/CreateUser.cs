namespace document_server2.Infrastructure.Comends
{
    public class CreateUser
    {
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
