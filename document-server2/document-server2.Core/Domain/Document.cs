using document_server2.Core.Domain.Entities;

namespace document_server2.Core.Domain
{
    public class doc : EntityInt
    {
        public int Case_id { get; private set; }
        public string Name { get; private set; }
        public string Url { get; private set; }

        protected doc()
        {

        }

        public doc(string name, string url)
        {
            Name = name;
            Url = url;
        }

        public void SetCaseid(int case_id)
        {

            Case_id = case_id;
        }

    }

    
}
