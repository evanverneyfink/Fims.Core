using System.Threading.Tasks;
using Fims.Azure.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fims.Azure.Services.Jobs.JobProcessor
{
    public static class JobProcessorFunctions
    {
        public static Task<IActionResult> HandleJobProcessorRequest([Inject] IFimsAzureResourceApi resourceApi, HttpRequest request)
        {
            return resourceApi.HandleRequest(request);
        }
    }
}
