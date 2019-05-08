using document_server2.Core.Domain.Entities;

namespace document_server2.Core.Domain
{
    public class User : Entity
    {
        public string Login { get; private set; }
        public string Password { get; private set; }
        public string Email { get; private set; }
        public int Role_id { get; private set; }
        public virtual Role Role { get; private set; }
    }
}
