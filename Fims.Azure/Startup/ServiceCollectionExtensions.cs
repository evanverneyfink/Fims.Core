using Fims.Azure.Http;
using Fims.Server;
using Fims.Server.Api;
using Fims.Server.Business;
using Fims.Server.Data;
using Fims.Server.Environment;
using Fims.Services.Jobs.WorkerFunctions;
using Microsoft.Extensions.DependencyInjection;

namespace Fims.Azure.Startup
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFimsAzure(this IServiceCollection services)
            => services
               .AddSingleton<ILogger, MicrosoftLoggerWrapper>()
               .AddEnvironment();

        /// <summary>
        /// Registers resource API-related services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="resourceHandlerRegistration"></param>
        /// <returns></returns>
        public static IServiceCollection AddFimsResourceApi(this IServiceCollection services, IResourceHandlerRegistration resourceHandlerRegistration)
            => services
               .AddFimsAzure()
               .AddFimsResourceDataHandling()
               .AddFimsResourceHandling(resourceHandlerRegistration)
               .AddFimsServerDefaultApi()
               .AddScoped<IRequest, HttpRequestWrapper>()
               .AddScoped<IFimsAzureResourceApi, FimsAzureResourceApi>();

        /// <summary>
        /// Registers resource API-related services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddFimsResourceApi<T>(this IServiceCollection services) where T : class, IResourceHandlerRegistration, new()
            => services.AddFimsResourceApi(new T());

        /// <summary>
        /// Registers resource API-related services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddFimsWorker<T>(this IServiceCollection services) where T : class, IWorker
            => services
               .AddFimsAzure()
               .AddSingleton<ILogger, MicrosoftLoggerWrapper>()
               .AddFimsResourceDataHandling()
               .AddScoped<IWorker, T>();
    }
}