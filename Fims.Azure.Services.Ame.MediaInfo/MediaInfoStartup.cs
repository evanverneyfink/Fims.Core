using Fims.Azure.Startup;
using Fims.Services.Ame.MediaInfo;
using Fims.Services.Jobs.WorkerFunctions;
using Microsoft.Extensions.DependencyInjection;

namespace Fims.Azure.Services.Ame.MediaInfo
{
    public class MediaInfoStartup : FimsServiceStartup
    {
        protected override IServiceCollection RegisterAdditionalServices(IServiceCollection services)
            => services.AddFimsResourceApi<WorkerFunctionInvocation<AzureFunctionWorkerInvoker>>()
                       .AddFimsWorker<MediaInfoWorker>();
    }
}