using System;
using Amazon.Lambda.Core;

namespace Fims.Aws.Lambda
{
    public class LambdaEnvironment : Server.Environment
    {
        /// <summary>
        /// Instantiates an <see cref="LambdaEnvironment"/>
        /// </summary>
        /// <param name="lambdaContext"></param>
        public LambdaEnvironment(ILambdaContext lambdaContext)
        {
            LambdaContext = lambdaContext;
        }

        /// <summary>
        /// Gets the lambda context
        /// </summary>
        private ILambdaContext LambdaContext { get; }

        /// <summary>
        /// Gets an environment variable
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override string GetTextValue(string key) =>
            Environment.GetEnvironmentVariables().Contains(key)
                ? (string)Environment.GetEnvironmentVariables()[key]
                : string.Empty;
    }
}
