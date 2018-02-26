using System;
using Fims.Server;
using Fims.Server.Api;

namespace Fims.Aws.ServiceBuilding
{
    internal class FimsAwsResourceApi : IFimsAwsResourceApi
    {
        /// <summary>
        /// Instantiates a <see cref="FimsAwsResourceApi"/>
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="logger"></param>
        /// <param name="requestHandler"></param>
        public FimsAwsResourceApi(IDisposable scope, ILogger logger, IRequestHandler requestHandler)
        {
            Scope = scope;
            Logger = logger;
            RequestHandler = requestHandler;
        }

        /// <summary>
        /// Gets the scope of the request
        /// </summary>
        private IDisposable Scope { get; }

        /// <summary>
        /// Gets the logger
        /// </summary>
        public ILogger Logger { get; }

        /// <summary>
        /// Gets the request handler
        /// </summary>
        public IRequestHandler RequestHandler { get; }

        /// <summary>
        /// Disposes of the underlying scope
        /// </summary>
        public void Dispose()
        {
            Scope?.Dispose();
        }
    }
}