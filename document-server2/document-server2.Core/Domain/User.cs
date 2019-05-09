using System.Collections.Generic;

namespace document_server2.Core.Domain
{
    public class User
    {
        public string Email { get; private set; }
        public string Login { get; private set; }
        public string Password { get; private set; }
        public string Role_name { get; private set; }
        public virtual Role Role { get; private set; }
        public virtual IEnumerable<Case> Cases { get; private set; }

        protected User()
        {

        }
        public User(string email, string login, string password, string role_name)
        {
            Email = email;
            Login = login;
            Password = password;
            Role_name = role_name;
        }
    }
}
