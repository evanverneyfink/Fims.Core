using Amazon.Lambda.APIGatewayEvents;
using Fims.Server.Api;

namespace Fims.Aws.Lambda.ApiGatewayProxy
{
    public interface IApiGatewayProxyLambdaResponse : IResponse
    {
        /// <summary>
        /// Gets the response as an <see cref="APIGatewayProxyResponse"/>
        /// </summary>
        /// <returns></returns>
        APIGatewayProxyResponse AsAwsApiGatewayProxyResponse();
    }
}