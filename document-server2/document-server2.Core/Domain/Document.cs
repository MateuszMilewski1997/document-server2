using document_server2.Core.Domain.Entities;

namespace document_server2.Core.Domain
{
    public class Document : EntityInt
    {
        public int Case_id { get; private set; }
        public string Name { get; private set; }
        public string Url { get; private set; }

        protected Document()
        {

        }

        public Document(string name, string url)
        {
            Name = name;
            Url = url;
        }
    } 
}
