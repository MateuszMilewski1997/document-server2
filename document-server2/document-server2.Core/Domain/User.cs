using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace document_server2.Core.Domain
{
    public class User
    {
        public string Email { get; private set; }
        public string Login { get; private set; }
        public string Password { get; private set; }
        public string Role_name { get; private set; }
        public virtual Role Role { get; private set; }
        private ISet<Case> _cases = new HashSet<Case>();
        public virtual IEnumerable<Case> Cases => _cases;

        protected User()
        {

        }
        public User(string email, string login, string password, string role_name)
        {
            SetEmail(email);
            SetLogin(login);
            SetPassword(password);
            SetRole(role_name);
        }

        public User(string email, string role_name)
        {
            SetEmail(email);
            SetRole(role_name);
        }

        private void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new Exception("Can not have an empty adress e-mail.");
            }
            if (email.Length > 50)
            {
                throw new Exception("Login can not have more than 50 characters.");
            }
            Regex syntax = new Regex("^[0-9a-zA-Z]+([.-]{1}[0-9a-zA-Z]+)*@[a-z]+([.-]{1}[0-9a-z]+)*.[a-z]*$");
            if (!syntax.IsMatch(email))
            {
                throw new Exception("The e-mail adress syntax is incorrect.");
            }

            Email = email;
        }

        public void SetLogin(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                throw new Exception("Can not have an empty login.");
            }
            if (login.Length < 5)
            {
                throw new Exception("Login must have more than 5 characters.");
            }
            if (login.Length > 20)
            {
                throw new Exception("Login can not have more than 20 characters.");
            }
            Regex syntax = new Regex("^[0-9a-zA-Z]{5,20}$");
            if (!syntax.IsMatch(login))
            {
                throw new Exception("The login syntax is incorrect.");
            }

            Login = login;
        }
        public void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Can not have an empty password.");
            }
            Password = password;
        }

        public void SetRole(string role_name)
        {
            if (string.IsNullOrWhiteSpace(role_name))
            {
                throw new Exception("Can not have an empty role name.");
            }
            if (role_name.Length > 20)
            {
                throw new Exception("Role name can not have more than 20 characters.");
            }
            Role_name = role_name;
        }

        public void AddCase(Case @case)
        {
            _cases.Add(@case);
        }
    }
}
