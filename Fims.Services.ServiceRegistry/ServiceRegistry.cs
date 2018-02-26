using Fims.Core.Model;
using Fims.Server.Business;

namespace Fims.Services.ServiceRegistry
{
    public class ServiceRegistry : IResourceHandlerRegistration
    {
        public void Register(ResourceHandlerRegistryOptions options)
        {
            options.Register<Service>()
                   .Register<JobProfile>();
        }
    }
}
