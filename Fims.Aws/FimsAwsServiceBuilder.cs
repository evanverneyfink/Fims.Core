using Fims.Aws.DynamoDb;
using Fims.Core;
using Fims.Server;
using Fims.Server.Api;
using Fims.Server.Business;
using Fims.Server.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Fims.Aws
{
    public class FimsAwsServiceBuilder
    {
        /// <summary>
        /// Instantiates a <see cref="FimsAwsServiceBuilder"/>
        /// </summary>
        /// <param name="serviceCollection"></param>
        private FimsAwsServiceBuilder(IServiceCollection serviceCollection)
        {
            ServiceCollecton = serviceCollection;
        }

        /// <summary>
        /// Gets the underlying service collection
        /// </summary>
        private IServiceCollection ServiceCollecton { get; }

        /// <summary>
        /// Creates a <see cref="FimsAwsServiceBuilder"/>
        /// </summary>
        /// <typeparam name="TEnv"></typeparam>
        /// <typeparam name="TReqCtx"></typeparam>
        /// <returns></returns>
        public static FimsAwsServiceBuilder Create<TEnv, TReqCtx>()
            where TEnv : class, IEnvironment
            where TReqCtx : class, IRequestContext
        {
            return new FimsAwsServiceBuilder(
                new ServiceCollection().AddFimsCore()
                                       .AddFimsResourceDataHandling()
                                       .AddFimsResourceHandling()
                                       .AddFimsServerDefaultApi()
                                       .AddScoped<IEnvironment, TEnv>()
                                       .AddScoped<IRequestContext, TReqCtx>());
        }

        /// <summary>
        /// Adds DynamoDB as the repository for the service
        /// </summary>
        /// <returns></returns>
        public FimsAwsServiceBuilder WithDynamoDbRepository()
        {
            ServiceCollecton.AddDynamoDbFimsRepository();
            return this;
        }

        /// <summary>
        /// Adds an object to the service collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public FimsAwsServiceBuilder With<T>(T obj) where T : class
        {
            ServiceCollecton.AddScoped(x => obj);
            return this;
        }

        /// <summary>
        /// Creates a request handler from a request
        /// </summary>
        /// <returns></returns>
        public IFimsAwsService Build()
        {
            var serviceProvider = ServiceCollecton.BuildServiceProvider();
            
            return new FimsAwsService(serviceProvider.CreateScope(), serviceProvider.GetRequiredService<ILogger>(), serviceProvider.GetRequiredService<IRequestHandler>());
        }
    }
}