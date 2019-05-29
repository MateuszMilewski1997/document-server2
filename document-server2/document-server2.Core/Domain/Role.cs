namespace document_server2.Core.Domain
{
    public class Role
    {
        public string Name { get; private set; }

        protected Role()
        {

        }

        public Role(string name)
        {
            Name = name;
        }
    }
}
