using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Fims.Aws.Lambda.ApiGatewayProxy;
using Fims.Services.Jobs.JobRepository;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Fims.Aws.Lambda
{
    public class JobRepositoryFunctions
    {
        /// <summary>
        /// FIMS handler for API Gateway proxy requests
        /// </summary>
        /// <param name="request"></param>
        /// <param name="lambdaContext"></param>
        /// <returns></returns>
        public async Task<APIGatewayProxyResponse> JobApi(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {
            return await LambdaApiGatewayProxy.Handle<JobRepository>(request, lambdaContext);
        }
    }
}
