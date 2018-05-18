using System.Net.Http;
using System.Threading.Tasks;
using Fims.Azure.DependencyInjection;

namespace Fims.Azure.Services.Jobs.JobProcessor
{
    public static class JobProcessorFunctions
    {
        public static Task<HttpResponseMessage> HandleJobProcessorRequest([Inject] IFimsAzureResourceApi resourceApi, HttpRequestMessage request)
        {
            return resourceApi.HandleRequest(request);
        }
    }
}
