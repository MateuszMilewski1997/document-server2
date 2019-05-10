using System.Collections.Generic;

namespace document_server2.Infrastructure.DTO
{
    public class CaseDetailsDTO : CaseDTO
    {
        public IEnumerable<DocumentDTO> Documents { get; set; }
        public class DocumentDTO
        {
            public string Name { get; set; }
            public string Url { get; set; }
        }
    }
}
