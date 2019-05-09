using document_server2.Core.Domain.Entities;
using System;
using System.Collections.Generic;

namespace document_server2.Core.Domain
{
    public class Case : EntityInt
    {
        public string User_email { get; private set; }
        public string Type { get; private set; }
        public DateTime Date { get; private set; }
        public string Description { get; private set; }
        public string Comment { get; private set; }
        public string Status { get; private set; }
        private ISet<Document> _documents = new HashSet<Document>();
        public virtual IEnumerable<Document> Documents => _documents;

        protected Case()
        {

        }
        public Case(string user_email, string type, string description)
        {
            User_email = user_email;
            Type = type;
            Date = DateTime.UtcNow;
            Description = description;
            Comment = string.Empty;
            Status = "not considered";
        }
    }
}
