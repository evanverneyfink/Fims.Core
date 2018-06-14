using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Fims.Azure.DependencyInjection;
using Fims.Server.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public static async Task<IActionResult> JobApi([Inject] IFimsAzureResourceApi resourceApi, HttpRequest request)
        {
            return await resourceApi.HandleRequest(request);
        }

        /// <summary>
        /// A Lambda function to run the MediaInfo worker
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static async Task<IActionResult> Worker([Inject] IFimsAzureWorker worker, HttpRequest request)
        {
            await worker.DoWork(request);
            return new OkResult();
        }
    }
}