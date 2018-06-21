using System.Threading.Tasks;
using Fims.Azure.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;

namespace Fims.Azure.Services.ServiceRegistry
{
    public static class ServiceRegistryFunctions
    {
        [FunctionName(nameof(ResourceApi))]
        public static Task<IActionResult> ResourceApi([HttpTrigger] HttpRequest request, [Inject] IFimsAzureResourceApi resourceApi)
        {
            return resourceApi.HandleRequest(request);
        }
    }
}