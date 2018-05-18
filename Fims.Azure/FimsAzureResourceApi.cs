using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Fims.Azure.Http;
using Fims.Server;
using Fims.Server.Api;

namespace Fims.Azure
{
    public class FimsAzureResourceApi : IFimsAzureResourceApi
    {
        /// <summary>
        /// Instantiates a <see cref="FimsAzureResourceApi"/>
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="requestHandler"></param>
        public FimsAzureResourceApi(ILogger logger, IRequestHandler requestHandler)
        {
            Logger = logger;
            RequestHandler = requestHandler;
        }

        /// <summary>
        /// Gets the logger
        /// </summary>
        private ILogger Logger { get; }

        /// <summary>
        /// Gets the request handler
        /// </summary>
        private IRequestHandler RequestHandler { get; }

        /// <summary>
        /// Handles a request to a FIMS resource API
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> HandleRequest(HttpRequestMessage request)
        {
            try
            {
                Logger.Info("Starting request handling...");

                // handle request
                return await RequestHandler.HandleRequest(new HttpRequestMessageRequest(request)) is IHttpResponseMessageResponse response
                           ? response.AsHttpResponseMessage()
                           : new HttpResponseMessage
                           {
                               StatusCode = HttpStatusCode.InternalServerError,
                               Content = new StringContent(
                                   "An unexpected error occurred. Internal configuration for API Gateway Proxy requests invalid.")
                           };
            }
            catch (Exception exception)
            {
                // log error
                Logger.Error($"An error occurred running API Gateway proxy lambda. Error: {exception}");

                // return unexpected 500
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent("An unexpected error occurred processing the request.")
                };
            }
        }
    }
}
