using System;
using System.Threading.Tasks;
using Amazon.Lambda;
using Fims.Core.Model;
using Fims.Core.Serialization;
using Fims.Server;
using Fims.Services.Jobs.WorkerFunctions;
using Microsoft.Extensions.Options;

namespace Fims.Aws.Lambda
{
    public class LambdaWorkerFunctionInvoker : IWorkerFunctionInvoker
    {
        /// <summary>
        /// Instantiates a <see cref="LambdaWorkerFunctionInvoker"/>
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="resourceSerializer"></param>
        /// <param name="options"></param>
        public LambdaWorkerFunctionInvoker(ILogger logger, IResourceSerializer resourceSerializer, IOptions<LambdaOptions> options)
        {
            Logger = logger;
            ResourceSerializer = resourceSerializer;

            // create client using credentials, if provided
            var region = options.Value?.RegionEndpoint;
            var creds = options.Value?.Credentials;
            Lambda = creds != null ? new AmazonLambdaClient(creds, region) : new AmazonLambdaClient();
        }

        /// <summary>
        /// Gets the logger
        /// </summary>
        private ILogger Logger { get; }

        /// <summary>
        /// Gets the resource serializer
        /// </summary>
        private IResourceSerializer ResourceSerializer { get; }

        /// <summary>
        /// Gets the lambda client
        /// </summary>
        private IAmazonLambda Lambda { get; }

        /// <summary>
        /// Invokes a lambda worker
        /// </summary>
        /// <param name="workerFunctionName"></param>
        /// <param name="environment"></param>
        /// <param name="jobAssignment"></param>
        /// <returns></returns>
        public async Task Invoke(string workerFunctionName, IEnvironment environment, JobAssignment jobAssignment)
        {
            try
            {
                Logger.Info("Invoking lambda function with name '{0}' for job assignment {1}...", workerFunctionName, jobAssignment.Id);

                await Lambda.InvokeAsync(
                    new Amazon.Lambda.Model.InvokeRequest
                    {
                        FunctionName = workerFunctionName,
                        InvocationType = "Event",
                        LogType = "None",
                        Payload = ResourceSerializer.Serialize(jobAssignment)
                    });

                Logger.Info("Invocation of lambda function with name '{0}' for job assignment {1} succeeded.", workerFunctionName, jobAssignment.Id);
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to invoke lambda function with name '{0}' for job assignment {1}. Exception: {2}",
                             workerFunctionName,
                             jobAssignment.Id,
                             ex);
                throw;
            }
        }
    }
}