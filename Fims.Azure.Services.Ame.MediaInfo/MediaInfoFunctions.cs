using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Fims.Azure.DependencyInjection;
using Fims.Server.Business;

namespace Fims.Azure.Services.Ame.MediaInfo
{
    public static class MediaInfoFunctions
    {
        /// <summary>
        /// A Lambda function to respond to API calls to create MediaInfo jobs
        /// </summary>
        /// <param name="resourceApi"></param>
        /// <param name="request"></param>
        /// <returns>The list of blogs</returns>
        public static async Task<HttpResponseMessage> JobApi([Inject] IFimsAzureResourceApi resourceApi, HttpRequestMessage request)
        {
            return await resourceApi.HandleRequest(request);
        }

        /// <summary>
        /// A Lambda function to run the MediaInfo worker
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> Worker([Inject] IFimsAzureWorker worker, HttpRequestMessage request)
        {
            await worker.DoWork(request);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}