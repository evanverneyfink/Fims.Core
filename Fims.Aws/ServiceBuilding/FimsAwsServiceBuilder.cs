﻿using Fims.Aws.DynamoDb;
using Fims.Aws.S3;
using Fims.Core.Serialization;
using Fims.Server;
using Fims.Server.Api;
using Fims.Server.Business;
using Fims.Server.Data;
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
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static FimsAwsServiceBuilder Create<T>()
            where T : class, IEnvironment
        {
            return new FimsAwsServiceBuilder(
                new ServiceCollection().AddFimsResourceDataHandling()
                                       .AddScoped<IEnvironment, T>());
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
        /// Builds a resource API
        /// </summary>
        /// <returns></returns>
        public IFimsAwsResourceApi BuildResourceApi<TRequestContext, TResourceRegistration>()
            where TRequestContext : class, IRequestContext
            where TResourceRegistration : IResourceHandlerRegistration, new()
        {
            ServiceCollection
                .AddSingleton<ILogger, ConsoleLogger>()
                .AddFimsResourceHandling<TResourceRegistration>()
                .AddFimsServerDefaultApi()
                .AddScoped<IRequestContext, TRequestContext>();

            var serviceProvider = ServiceCollection.BuildServiceProvider();

            return new FimsAwsResourceApi(serviceProvider.CreateScope(),
                                          serviceProvider.GetRequiredService<ILogger>(),
                                          serviceProvider.GetRequiredService<IRequestHandler>());
        }

        /// <summary>
        /// Creates a builds a worker service
        /// </summary>
        /// <returns></returns>
        public IFimsAwsWorkerService BuildWorkerSevice<T>()
            where T : class, IWorker
        {
            ServiceCollection.AddScoped<IWorker, T>();

            var serviceProvider = ServiceCollection.BuildServiceProvider();

            return new FimsAwsWorkerService(serviceProvider.CreateScope(),
                                            serviceProvider.GetRequiredService<ILogger>(),
                                            serviceProvider.GetRequiredService<IWorker>(),
                                            serviceProvider.GetRequiredService<IResourceSerializer>());
        }
    }
}