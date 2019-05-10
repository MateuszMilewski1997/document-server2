namespace document_server2.Infrastructure.Comends
{
    public class UpdateUser : CreateUser
    {
        public string NewPassword { get; set; }
    }
}
