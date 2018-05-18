using Fims.Azure.Startup;
using Microsoft.Extensions.DependencyInjection;

namespace Fims.Azure.Services.Jobs.JobProcessor
{
    public class JobProcessoStartup : FimsServiceStartup
    {
        protected override IServiceCollection RegisterAdditionalServices(IServiceCollection services)
            => services.AddFimsResourceApi<Fims.Services.Jobs.JobProcessor.JobProcessor>();
    }
}
