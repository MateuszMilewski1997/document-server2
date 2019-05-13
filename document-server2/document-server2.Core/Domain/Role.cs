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

        public Role(string name, bool add_documents, bool add_comments, bool change_status, bool add_users)
        {
            Name = name;
            Add_documents = add_documents;
            Add_comments = add_comments;
            Change_status = change_status;
            Add_users = add_users;
        }
    }
}
