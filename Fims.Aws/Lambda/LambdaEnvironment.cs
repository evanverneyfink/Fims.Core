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
        protected override string GetTextValue(string key) => Environment.GetEnvironmentVariables().Contains(key)
                                                                  ? (string)Environment.GetEnvironmentVariables()[key]
                                                                  : string.Empty;

        /// <summary>
        /// Sets th text value for a config key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected override void SetTextValue(string key, string value) => Environment.SetEnvironmentVariable(key, value);
    }
}
