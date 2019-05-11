using document_server2.Core.Domain.Entities;
using System;
using System.Text.RegularExpressions;

namespace document_server2.Core.Domain
{
    public class Recipient : EntityInt
    {
        public int Case_id { get; private set; }
        public string Email { get; private set; }

        protected Recipient()
        {
            
        }

        public Recipient(string email)
        {
            SetEmail(email);
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
    }
}
