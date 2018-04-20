using System.Threading.Tasks;
using Fims.Core.Model;
using Fims.Server;

namespace Fims.Services.Jobs.WorkerFunctions
{
    public interface IWorkerFunctionInvoker
    {
        /// <summary>
        /// Invokes a worker function
        /// </summary>
        /// <param name="workerFunctionName"></param>
        /// <param name="environment"></param>
        /// <param name="jobAssignment"></param>
        /// <returns></returns>
        Task Invoke(string workerFunctionName, IEnvironment environment, JobAssignment jobAssignment);
    }
}