using Fims.Azure.Startup;
using Microsoft.Extensions.DependencyInjection;

namespace Fims.Azure.Services.ServiceRegistry
{
    public class ServiceRegistryStartup : FimsServiceStartup
    {
        protected override IServiceCollection RegisterAdditionalServices(IServiceCollection services)
            => services.AddFimsResourceApi<Fims.Services.ServiceRegistry.ServiceRegistry>();
    }
}
