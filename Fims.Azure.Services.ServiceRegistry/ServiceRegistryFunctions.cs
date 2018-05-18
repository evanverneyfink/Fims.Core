using System.Net.Http;
using System.Threading.Tasks;
using Fims.Azure.DependencyInjection;

namespace Fims.Azure.Services.ServiceRegistry
{
    public static class ServiceRegistryFunctions
    {
        public static Task<HttpResponseMessage> HandleServiceRegistryRequest([Inject] IFimsAzureResourceApi resourceApi, HttpRequestMessage request)
        {
            return resourceApi.HandleRequest(request);
        }
    }
}