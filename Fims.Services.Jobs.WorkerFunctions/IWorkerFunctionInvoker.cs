using System.Threading.Tasks;
using Fims.Core.Model;
using Fims.Server.Environment;

namespace Fims.Services.Jobs.WorkerFunctions
{
    public interface IWorkerFunctionInvoker
    {
        /// <summary>
        /// Invokes a worker function
        /// </summary>
        /// <param name="workerFunctionId"></param>
        /// <param name="environment"></param>
        /// <param name="jobAssignment"></param>
        /// <returns></returns>
        Task Invoke(string workerFunctionId, IEnvironment environment, JobAssignment jobAssignment);
    }
}