using System.Net.Http;
using System.Threading.Tasks;

namespace Fims.Azure
{
    public interface IFimsAzureWorker
    {
        /// <summary>
        /// Executes a worker using the content provided in an HTTP request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task DoWork(HttpRequestMessage request);
    }
}