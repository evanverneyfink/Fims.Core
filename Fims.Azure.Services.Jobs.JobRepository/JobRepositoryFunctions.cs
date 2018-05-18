using System;
using System.Net.Http;
using System.Threading.Tasks;
using Fims.Azure.DependencyInjection;

namespace Fims.Azure.Services.Jobs.JobRepository
{
    public static class JobRepositoryFunctions
    {
        public static Task<HttpResponseMessage> HandleJobRepositoryRequest([Inject] IFimsAzureResourceApi resourceApi, HttpRequestMessage request)
        {
            return resourceApi.HandleRequest(request);
        }
    }
}
