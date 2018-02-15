using System;
using System.Collections.Generic;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Fims.Server;

namespace Fims.Aws.Lambda.ApiGatewayProxy
{
    public class ApiGatewayProxyLambdaEnvironment : IEnvironment
    {
        /// <summary>
        /// Instantiates an <see cref="ApiGatewayProxyLambdaEnvironment"/>
        /// </summary>
        /// <param name="lambdaContext"></param>
        /// <param name="request"></param>
        public ApiGatewayProxyLambdaEnvironment(ILambdaContext lambdaContext, APIGatewayProxyRequest request)
        {
            LambdaContext = lambdaContext;
            StageVariables = request.StageVariables ?? new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets the lambda context
        /// </summary>
        private ILambdaContext LambdaContext { get; }
        
        /// <summary>
        /// Gets the stage variables
        /// </summary>
        private IDictionary<string, string> StageVariables { get; }

        /// <summary>
        /// Gets the root path of the url
        /// </summary>
        public string RootPath => Environment.GetEnvironmentVariables().Contains(nameof(RootPath))
                                      ? (string)Environment.GetEnvironmentVariables()[nameof(RootPath)]
                                      : string.Empty;

        /// <summary>
        /// Gets the base public url for the server
        /// </summary>
        public string PublicUrl => StageVariables.ContainsKey(nameof(PublicUrl)) ? StageVariables[nameof(PublicUrl)] : null;
    }
}