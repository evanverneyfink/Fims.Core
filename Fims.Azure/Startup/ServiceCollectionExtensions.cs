using System;
using Fims.Azure.Http;
using Fims.Server;
using Fims.Server.Api;
using Fims.Server.Business;
using Fims.Server.Data;
using Fims.Server.Environment;
using Fims.Services.Jobs.WorkerFunctions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fims.Azure.Startup
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFimsAzure(this IServiceCollection services, Action<IConfigurationBuilder> addConfig = null)
        {
            // build config
            var configBuilder = new ConfigurationBuilder();
            addConfig?.Invoke(configBuilder);
            var config = configBuilder.Build();

            // add logging and config
            services
                .AddSingleton<ILogger, MicrosoftLoggerWrapper>()
                .AddEnvironment(opts => opts.AddProvider(new ConfigValueProvider(config)));

            return services;
        }

        /// <summary>
        /// Registers resource API-related services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="resourceHandlerRegistration"></param>
        /// <param name="addConfig"></param>
        /// <returns></returns>
        public static IServiceCollection AddFimsResourceApi(this IServiceCollection services,
                                                            IResourceHandlerRegistration resourceHandlerRegistration,
                                                            Action<IConfigurationBuilder> addConfig = null)
            => services
               .AddFimsAzure(addConfig)
               .AddFimsResourceDataHandling()
               .AddFimsResourceHandling(resourceHandlerRegistration)
               .AddFimsServerDefaultApi()
               .AddScoped<IRequest, HttpRequestWrapper>()
               .AddScoped<IFimsAzureResourceApi, FimsAzureResourceApi>();

        /// <summary>
        /// Registers resource API-related services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="addConfig"></param>
        /// <returns></returns>
        public static IServiceCollection AddFimsResourceApi<T>(this IServiceCollection services, Action<IConfigurationBuilder> addConfig = null)
            where T : class, IResourceHandlerRegistration, new()
            => services.AddFimsResourceApi(new T(), addConfig);

        /// <summary>
        /// Registers resource API-related services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="addConfig"></param>
        /// <returns></returns>
        public static IServiceCollection AddFimsWorker<T>(this IServiceCollection services, Action<IConfigurationBuilder> addConfig = null)
            where T : class, IWorker
            => services
               .AddFimsAzure(addConfig)
               .AddSingleton<ILogger, MicrosoftLoggerWrapper>()
               .AddFimsResourceDataHandling()
               .AddScoped<IWorker, T>();
    }
}