using System.Threading.Tasks;
using Fims.Azure.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;

namespace Fims.Azure.Services.ServiceRegistry
{
    public static class ServiceRegistryFunctions
    {
        [FunctionName(nameof(HandleRequest))]
        public static Task<IActionResult> HandleRequest([HttpTrigger] HttpRequest request, [Inject] IFimsAzureResourceApi resourceApi)
        {
            return Task.FromResult<IActionResult>(new OkObjectResult("You did it!"));

            //return resourceApi.HandleRequest(request);
        }
    }
}