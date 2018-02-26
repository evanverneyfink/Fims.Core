namespace Fims.Core.Model
{
    public class IdResource : Resource
    {
        public IdResource()
            : base("@id")
        {
        }

        public IdResource(string id)
            : this()
        {
            Id = id;
        }

        public static implicit operator string(IdResource value) => value?.Id;

        public static implicit operator IdResource(string value) => new IdResource { Id = value };
    }
}