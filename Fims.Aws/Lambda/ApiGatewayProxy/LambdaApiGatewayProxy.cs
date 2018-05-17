using System;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Fims.Aws.ServiceBuilding;
using Fims.Server.Business;

namespace Fims.Aws.Lambda.ApiGatewayProxy
{
    public static class LambdaApiGatewayProxy
    {
        /// <summary>
        /// Handles an API Gateway proxy request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lambdaContext"></param>
        /// <param name="request"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static async Task<APIGatewayProxyResponse> Handle<T>(APIGatewayProxyRequest request,
                                                                    ILambdaContext lambdaContext,
                                                                    Action<FimsAwsServiceBuilder> configure = null)
            where T : IResourceHandlerRegistration, new()
        {
            IFimsAwsResourceApi service = null;
            try
            {
                Console.WriteLine("Building FIMS service...");

                // build service
                var serviceBuilder = FimsAwsServiceBuilder.Create<ApiGatewayProxyLambdaEnvironment>()
                                               .WithDynamoDbRepository()
                                               .WithS3FileStorage()
                                               .With(request)
                                               .With(lambdaContext);

                configure?.Invoke(serviceBuilder);
                    
                service = serviceBuilder.BuildResourceApi<ApiGatewayProxyLambdaRequestContext, T>();
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
                    Body = "An unexpected error occurred building the service."
                };
            }

            try
            {
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
                service.Logger.Error($"An error occurred running API Gateway proxy lambda. Error: {exception}");

                // return unexpected 500
                return new APIGatewayProxyResponse
                {
                    StatusCode = 500,
                    Body = "An unexpected error occurred processing the request."
                };
            }
            finally
            {
                service.Dispose();
            }
        }
    }
}
