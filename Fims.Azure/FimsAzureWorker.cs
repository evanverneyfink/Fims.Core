using System.Net.Http;
using System.Threading.Tasks;
using Fims.Core.Model;
using Fims.Core.Serialization;
using Fims.Services.Jobs.WorkerFunctions;

namespace Fims.Azure
{
    public class FimsAzureWorker : IFimsAzureWorker
    {
        /// <summary>
        /// Instantiates a <see cref="FimsAzureWorker"/>
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="resourceSerializer"></param>
        public FimsAzureWorker(IWorker worker, IResourceSerializer resourceSerializer)
        {
            Worker = worker;
            ResourceSerializer = resourceSerializer;
        }

        /// <summary>
        /// Gets the worker
        /// </summary>
        private IWorker Worker { get; }

        /// <summary>
        /// Gets the resource serializer
        /// </summary>
        private IResourceSerializer ResourceSerializer { get; }

        /// <summary>
        /// Executes a worker using the content provided in an HTTP request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task DoWork(HttpRequestMessage request)
        {
            await Worker.Execute(await ResourceSerializer.Deserialize<JobAssignment>(await request.Content.ReadAsStringAsync()));
        }
    }
}