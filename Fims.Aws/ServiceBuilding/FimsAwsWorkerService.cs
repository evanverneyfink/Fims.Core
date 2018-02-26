using System;
using Fims.Server;
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
        public FimsAwsWorkerService(IDisposable scope, ILogger logger, IWorker worker)
        {
            Scope = scope;
            Logger = logger;
            Worker = worker;
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
        /// Disposes of the underlying worker
        /// </summary>
        public void Dispose()
        {
            Scope?.Dispose();
        }
    }
}