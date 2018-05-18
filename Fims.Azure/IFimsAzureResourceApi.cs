using System.Net.Http;
using System.Threading.Tasks;

namespace Fims.Azure
{
    public interface IFimsAzureResourceApi
    {
        /// <summary>
        /// Handles a request to a FIMS resource API
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> HandleRequest(HttpRequestMessage request);
    }
}