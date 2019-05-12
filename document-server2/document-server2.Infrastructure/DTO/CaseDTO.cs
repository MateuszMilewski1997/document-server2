using System;

namespace document_server2.Infrastructure.DTO
{
    public class CaseDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string User_email { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public string Status { get; set; }
    }
}
