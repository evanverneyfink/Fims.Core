using System;
using System.Threading.Tasks;
using Fims.Azure.Http;
using Fims.Server;
using Fims.Server.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> HandleRequest(HttpRequest request)
        {
            try
            {
                Logger.Info("Starting request handling...");

                // handle request
                return
                    await RequestHandler.HandleRequest(new HttpRequestWrapper(request)) is IActionResultResponse responseWrapper
                        ? responseWrapper.AsActionResult()
                        : throw new Exception(
                              "An error occurred running the resource API Azure function. Internal configuration for Azure Function requests invalid.");
            }
            catch (Exception exception)
            {
                // log error
                Logger.Error($"An error occurred running the resource API Azure Function. Error: {exception}");

                throw;
            }
        }
    }
}
