using Fims.Services.Jobs.WorkerFunctions;

namespace Fims.Aws.ServiceBuilding
{
    public interface IFimsAwsWorkerService : IFimsService
    {
        /// <summary>
        /// Gets the worker
        /// </summary>
        IWorker Worker { get; }
    }
}