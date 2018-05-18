using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Fims.Aws.ServiceBuilding;
using Fims.Core.Model;
using Fims.Services.Jobs.WorkerFunctions;

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
        /// <param name="configure"></param>
        /// <returns></returns>
        public static async Task Handle<T>(Stream input, ILambdaContext lambdaContext, Action<FimsAwsServiceBuilder> configure = null) where T : class, IWorker
        {
            IFimsAwsWorkerService service = null;
            try
            {
                // build worker service
                var serviceBuilder =
                    FimsAwsServiceBuilder.Create()
                                         .WithDynamoDbRepository()
                                         .WithS3FileStorage()
                                         .With(lambdaContext);

                configure?.Invoke(serviceBuilder);

                service = serviceBuilder.BuildWorkerSevice<T>();
                
                // read input as text and deserialize it
                var jobAssignment = await service.ResourceSerializer.Deserialize<JobAssignment>(await new StreamReader(input).ReadToEndAsync());

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
