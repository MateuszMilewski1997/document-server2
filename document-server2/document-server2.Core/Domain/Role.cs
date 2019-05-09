namespace document_server2.Core.Domain
{
    public class Role
    {
        public string Name { get; private set; }
        public bool Add_documents { get; private set; }
        public bool Add_comments { get; private set; }
        public bool Change_status { get; private set; }
        public bool Add_users { get; private set; }

        protected Role()
        {

        }
    }
}
