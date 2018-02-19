namespace Fims.Core.Model
{
    public class ServiceResource : Resource
    {
        public ServiceResource()
        {
        }

        public ServiceResource(string resourceType, string httpEndpoint)
        {
            ResourceType = resourceType;
            HttpEndpoint = httpEndpoint;
        }

        public string ResourceType
        {
            get => GetString(nameof(ResourceType));
            set => Set(nameof(ResourceType), value);
        }

        public string HttpEndpoint
        {
            get => GetString(nameof(HttpEndpoint));
            set => Set(nameof(HttpEndpoint), value);
        }
    }
}