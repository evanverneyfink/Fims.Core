using Fims.Azure.Startup;
using Fims.Server.Business;
using Microsoft.Extensions.DependencyInjection;

namespace Fims.Azure.Services.Jobs.JobRepository
{
    public class JobRepositoryStartup : FimsServiceStartup
    {
        protected override IServiceCollection RegisterAdditionalServices(IServiceCollection services)
            => services.AddFimsResourceApi<Fims.Services.Jobs.JobRepository.JobRepository>();
    }
}