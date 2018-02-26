using System;
using Amazon.Lambda.Core;
using Fims.Core;
using Fims.Server;

namespace Fims.Aws.Lambda
{
    public class LambdaEnvironment : IEnvironment
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
        /// Gets a setting from the API Gateway proxy lambda environment
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual T Get<T>(string key)
        {
            var val = GetEnvironmentVariable(key);

            return val != null ? val.Parse<T>() : default(T);
        }

        /// <summary>
        /// Gets an environment variable
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetEnvironmentVariable(string key) =>
            Environment.GetEnvironmentVariables().Contains(key)
                ? (string)Environment.GetEnvironmentVariables()[key]
                : string.Empty;
    }
}
