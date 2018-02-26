using System.Collections.Generic;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Fims.Core;

namespace Fims.Aws.Lambda.ApiGatewayProxy
{
    public class ApiGatewayProxyLambdaEnvironment : LambdaEnvironment
    {
        /// <summary>
        /// Instantiates an <see cref="ApiGatewayProxyLambdaEnvironment"/>
        /// </summary>
        /// <param name="lambdaContext"></param>
        /// <param name="request"></param>
        public ApiGatewayProxyLambdaEnvironment(ILambdaContext lambdaContext, APIGatewayProxyRequest request)
            : base(lambdaContext)
        {
            StageVariables = request.StageVariables ?? new Dictionary<string, string>();
        }
        
        /// <summary>
        /// Gets the stage variables
        /// </summary>
        private IDictionary<string, string> StageVariables { get; }

        /// <summary>
        /// Gets a setting from the API Gateway proxy lambda environment
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public override T Get<T>(string key)
        {
            if (StageVariables.ContainsKey(key))
                return StageVariables[key].Parse<T>();

            return base.Get<T>(key);
        }
    }
}