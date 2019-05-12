using System.Collections.Generic;

namespace document_server2.Infrastructure.Comends
{
    public class UpdateCase
    {
        public string Comment { get; set; }
        public string Status { get; set; }

        public IEnumerable<SendDocument> Documents { get; set; }

        public class SendDocument
        {
            public string Name { get; set; }
            public string Url { get; set; }
        }
    }
}
