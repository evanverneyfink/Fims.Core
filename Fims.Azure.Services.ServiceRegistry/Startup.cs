using Fims.Azure.Startup;
using Microsoft.Extensions.DependencyInjection;

namespace Fims.Azure.Services.ServiceRegistry
{
    public class Startup : IStartup
    {
        public IServiceCollection Configure(IServiceCollection services) => services.AddFimsResourceApi<Fims.Services.ServiceRegistry.ServiceRegistry>();
    }
}
