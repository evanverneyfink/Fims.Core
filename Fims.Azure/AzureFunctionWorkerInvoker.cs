﻿using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Fims.Core.Model;
using Fims.Core.Serialization;
using Fims.Server.Environment;
using Fims.Services.Jobs.WorkerFunctions;

namespace Fims.Azure
{
    public class AzureFunctionWorkerInvoker : IWorkerFunctionInvoker
    {
        /// <summary>
        /// Instantiates an <see cref="AzureFunctionWorkerInvoker"/>
        /// </summary>
        /// <param name="resourceSerializer"></param>
        public AzureFunctionWorkerInvoker(IResourceSerializer resourceSerializer)
        {
            ResourceSerializer = resourceSerializer;
        }

        /// <summary>
        /// Gets the resource sserializer
        /// </summary>
        private IResourceSerializer ResourceSerializer { get; }

        /// <summary>
        /// Gets the HTTP client used to invoke Azure Functions via HTTP
        /// </summary>
        private HttpClient HttpClient { get; } = new HttpClient();

        /// <summary>
        /// Invokes a worker function
        /// </summary>
        /// <param name="workerFunctionId"></param>
        /// <param name="environment"></param>
        /// <param name="jobAssignment"></param>
        /// <returns></returns>
        public async Task Invoke(string workerFunctionId, IEnvironment environment, JobAssignment jobAssignment)
        {
            await HttpClient.PostAsync(workerFunctionId,
                                       new StringContent(ResourceSerializer.Serialize(jobAssignment), Encoding.UTF8, "application/json"));
        }
    }
}