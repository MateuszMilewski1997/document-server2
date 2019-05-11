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
        private ISet<Recipient> _recipients = new HashSet<Recipient>();
        public virtual IEnumerable<Recipient> Recipients => _recipients;
        protected Case()
        {

        }
        public Case(string user_email, string type, string description, IEnumerable<Document> documents, IEnumerable<string> recipients)
        {
            User_email = user_email;
            Type = type;
            Date = DateTime.UtcNow;
            Description = description;
            Comment = string.Empty;
            Status = "not considered";

            foreach (Document document in documents)
            {
                _documents.Add(new Document(document.Name, document.Url));
            }

            foreach (string email in recipients)
            {
                _recipients.Add(new Recipient(email));
            }
        }

        public void SetStatus(string status)
        {
            if(status == null)
            {
                return;
            }
            Status = status;
        }

        public void SetComment(string comment)
        {
            if (comment == null)
            {
                return;
            }
            Comment = comment;
        }
    }
}
