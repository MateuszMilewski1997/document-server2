﻿using System.Collections.Generic;

namespace document_server2.Infrastructure.Comends
{
    public class CreateCase
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public IEnumerable<SendDocument> Documents { get; set; }
        public IEnumerable<string> Recipients { get; set; }

        public class SendDocument
        {
            public string Name { get; set; }
            public string Url { get; set; }
        }
    }
}
