using System;
using Fims.Core.Serialization;
using Fims.Server;
using Fims.Server.Api;
using Fims.Services.Jobs.WorkerFunctions;

namespace Fims.Aws.ServiceBuilding
{
    public class FimsAwsWorkerService : IFimsAwsWorkerService
    {
        /// <summary>
        /// Instantiates a <see cref="FimsAwsWorkerService"/>
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="logger"></param>
        /// <param name="worker"></param>
        /// <param name="resourceSerializer"></param>
        public FimsAwsWorkerService(IDisposable scope, ILogger logger, IWorker worker, IResourceSerializer resourceSerializer)
        {
            Scope = scope;
            Logger = logger;
            Worker = worker;
            ResourceSerializer = resourceSerializer;
        }

        /// <summary>
        /// Gets the scope
        /// </summary>
        private IDisposable Scope { get; }

        /// <summary>
        /// Gets the logger
        /// </summary>
        public ILogger Logger { get; }

        /// <summary>
        /// Gets the worker
        /// </summary>
        public IWorker Worker { get; }

        /// <summary>
        /// Gets the resource serializer
        /// </summary>
        public IResourceSerializer ResourceSerializer { get; }

        /// <summary>
        /// Disposes of the underlying worker
        /// </summary>
        public void Dispose()
        {
            Scope?.Dispose();
        }
    }
}