using System;
using Fims.Aws.DynamoDb;
using Fims.Aws.S3;
using Fims.Core.Serialization;
using Fims.Server;
using Fims.Server.Api;
using Fims.Server.Business;
using Fims.Server.Data;
using Fims.Server.Environment;
using Fims.Services.Jobs.WorkerFunctions;
using Microsoft.Extensions.DependencyInjection;

namespace Fims.Aws.ServiceBuilding
{
    public class FimsAwsServiceBuilder
    {
        /// <summary>
        /// Instantiates a <see cref="FimsAwsServiceBuilder"/>
        /// </summary>
        /// <param name="serviceCollection"></param>
        private FimsAwsServiceBuilder(IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection;
        }

        /// <summary>
        /// Gets the underlying service collection
        /// </summary>
        private IServiceCollection ServiceCollection { get; }

        /// <summary>
        /// Creates a <see cref="FimsAwsServiceBuilder"/>
        /// </summary>
        /// <returns></returns>
        public static FimsAwsServiceBuilder Create(Action<EnvironmentOptions> configureEnvironmentVariables = null)
        {
            return new FimsAwsServiceBuilder(
                new ServiceCollection().AddFimsResourceDataHandling()
                                       .AddEnvironment(configureEnvironmentVariables));
        }

        /// <summary>
        /// Adds DynamoDB as the repository for the service
        /// </summary>
        /// <returns></returns>
        public FimsAwsServiceBuilder WithDynamoDbRepository()
        {
            ServiceCollection.AddDynamoDbFimsRepository();
            return this;
        }

        /// <summary>
        /// Adds S3 file storage for the service
        /// </summary>
        /// <returns></returns>
        public FimsAwsServiceBuilder WithS3FileStorage()
        {
            ServiceCollection.AddS3FileStorage();
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
            ServiceCollection.AddScoped(x => obj);
            return this;
        }

        /// <summary>
        /// Adds an object to the service collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public FimsAwsServiceBuilder With<T>() where T : class
        {
            ServiceCollection.AddScoped<T>();
            return this;
        }

        /// <summary>
        /// Adds a type registration
        /// </summary>
        /// <typeparam name="TRegistered"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <returns></returns>
        public FimsAwsServiceBuilder With<TRegistered, TImplementation>()
            where TRegistered : class
            where TImplementation : class, TRegistered
        {
            ServiceCollection.AddScoped<TRegistered, TImplementation>();
            return this;
        }

        /// <summary>
        /// Adds an object to the service collection
        /// </summary>
        /// <returns></returns>
        public FimsAwsServiceBuilder With(Action<IServiceCollection> register)
        {
            register(ServiceCollection);
            return this;
        }

        /// <summary>
        /// Builds a resource API
        /// </summary>
        /// <returns></returns>
        public IFimsAwsResourceApi BuildResourceApi<TRequestContext, TResourceRegistration>(Action<IServiceCollection> addAdditionalServices = null)
            where TRequestContext : class, IRequest
            where TResourceRegistration : IResourceHandlerRegistration, new()
        {
            ServiceCollection
                .AddSingleton<ILogger, ConsoleLogger>()
                .AddFimsResourceHandling<TResourceRegistration>()
                .AddFimsServerDefaultApi()
                .AddScoped<IRequest, TRequestContext>();

            addAdditionalServices?.Invoke(ServiceCollection);

            var serviceProvider = ServiceCollection.BuildServiceProvider();

            return new FimsAwsResourceApi(serviceProvider.CreateScope(),
                                          serviceProvider.GetRequiredService<ILogger>(),
                                          serviceProvider.GetRequiredService<IRequestHandler>());
        }

        /// <summary>
        /// Creates a builds a worker service
        /// </summary>
        /// <returns></returns>
        public IFimsAwsWorkerService BuildWorkerSevice<T>(Action<IServiceCollection> addAdditionalServices = null)
            where T : class, IWorker
        {
            ServiceCollection
                .AddSingleton<ILogger, ConsoleLogger>()
                .AddScoped<IWorker, T>();

            addAdditionalServices?.Invoke(ServiceCollection);

            var serviceProvider = ServiceCollection.BuildServiceProvider();

            return new FimsAwsWorkerService(serviceProvider.CreateScope(),
                                            serviceProvider.GetRequiredService<ILogger>(),
                                            serviceProvider.GetRequiredService<IWorker>(),
                                            serviceProvider.GetRequiredService<IResourceSerializer>());
        }
    }
}