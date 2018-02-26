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
        /// <typeparam name="T"></typeparam>
        /// <param name="workerFunctionName"></param>
        /// <param name="environment"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        Task Invoke<T>(string workerFunctionName, IEnvironment environment, T resource) where T : Resource;
    }
}