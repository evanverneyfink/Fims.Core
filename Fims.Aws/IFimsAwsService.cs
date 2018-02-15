using System;
using Fims.Server;
using Fims.Server.Api;

namespace Fims.Aws
{
    public interface IFimsAwsService : IDisposable
    {
        /// <summary>
        /// Gets the logger
        /// </summary>
        ILogger Logger { get; }

        /// <summary>
        /// Gets the request handler
        /// </summary>
        IRequestHandler RequestHandler { get; }
    }
}