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

        public string ResourceType { get; set; }

        public string HttpEndpoint { get; set; }
    }
}