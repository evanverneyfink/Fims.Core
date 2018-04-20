using System.Threading.Tasks;
using Fims.Core.Model;

namespace Fims.Services.Jobs.WorkerFunctions
{
    public interface IWorker
    {
        /// <summary>
        /// Executes work for the given input
        /// </summary>
        /// <param name="jobAssignment"></param>
        /// <returns></returns>
        Task Execute(JobAssignment jobAssignment);
    }
}
