using System;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

namespace Fims.Aws.Lambda.ApiGatewayProxy
{
    public class ApiGatewayProxyLambdaHandler
    {
        /// <summary>
        /// Builds a <see cref="IFimsAwsService"/> for an <see cref="APIGatewayProxyRequest"/>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="lambdaContext"></param>
        /// <returns></returns>
        public static IFimsAwsService BuildService(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {
            return FimsAwsServiceBuilder.Create<ApiGatewayProxyLambdaEnvironment, ApiGatewayProxyLambdaRequestContext>()
                                        .WithDynamoDbRepository()
                                        .With(request)
                                        .With(lambdaContext)
                                        .Build();
        }

        /// <summary>
        /// Handles an API Gateway proxy request
        /// </summary>
        /// <param name="lambdaContext"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static async Task<APIGatewayProxyResponse> Handle(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {
            IFimsAwsService service = null;
            try
            {
                Console.WriteLine("Building FIMS service...");

                // build service
                service = BuildService(request, lambdaContext);

                service.Logger.Info("Service built successfully. Starting request handling...");

                // handle request
                return await service.RequestHandler.HandleRequest() is IApiGatewayProxyLambdaResponse response
                           ? response.AsAwsApiGatewayProxyResponse()
                           : new APIGatewayProxyResponse
                           {
                               StatusCode = 500,
                               Body = "An unexpected error occurred. Internal configuration for API Gateway Proxy requests invalid."
                           };
            }
            catch (Exception exception)
            {
                // log error
                var message = $"An error occurred running API Gateway proxy lambda. Error: {exception}";
                if (service?.Logger != null)
                    service.Logger.Error(message);
                else
                    Console.WriteLine(message);

                // return unexpected 500
                return new APIGatewayProxyResponse
                {
                    StatusCode = 500,
                    Body = "An unexpected error occurred. Internal configuration for API Gateway Proxy requests invalid."
                };
            }
            finally
            {
                service?.Dispose();
            }
        }
    }
}
