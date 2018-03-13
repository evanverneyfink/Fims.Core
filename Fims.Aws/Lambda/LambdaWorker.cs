using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Fims.Aws.ServiceBuilding;
using Fims.Core.Model;
using Fims.Services.Jobs.WorkerFunctions;
using Newtonsoft.Json;

namespace Fims.Aws.Lambda
{
    public static class LambdaWorker
    {
        /// <summary>
        /// Runs a lambda worker
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="lambdaContext"></param>
        /// <returns></returns>
        public static async Task Handle<T>(dynamic input, ILambdaContext lambdaContext) where T : class, IWorker
        {
            IFimsAwsWorkerService service = null;
            try
            {
                // build worker service
                service =
                    FimsAwsServiceBuilder.Create<LambdaEnvironment>()
                                         .WithDynamoDbRepository()
                                         .WithS3FileStorage()
                                         .With(lambdaContext)
                                         .BuildWorkerSevice<T>();

                // use JSON serialization to convert input object to JobAssignment
                var jobAssignment = service.ResourceSerializer.Deserialize<JobAssignment>(JsonConvert.SerializeObject(input));

                // run worker
                await service.Worker.Execute(jobAssignment);
            }
            finally
            {
                service?.Dispose();
            }
        }
    }
}
