using System;
using Fims.Server;
using Fims.Server.Api;

namespace Fims.Aws
{
    internal class FimsAwsService : IFimsAwsService
    {
        /// <summary>
        /// Instantiates a <see cref="FimsAwsService"/>
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="logger"></param>
        /// <param name="requestHandler"></param>
        public FimsAwsService(IDisposable scope, ILogger logger, IRequestHandler requestHandler)
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