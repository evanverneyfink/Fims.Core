using System.Collections.Generic;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

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
        /// <param name="key"></param>
        /// <returns></returns>
        protected override string GetTextValue(string key)
        {
            if (StageVariables.ContainsKey(key))
                return StageVariables[key];

            return base.GetTextValue(key);
        }
    }
}