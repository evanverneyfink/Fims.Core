using Fims.Azure.Http;
using Fims.Server.Api;
using Fims.Server.Business;
using Fims.Services.Jobs.WorkerFunctions;
using Microsoft.Extensions.DependencyInjection;

namespace Fims.Azure.Startup
{
    public static class StartupExtensions
    {
        /// <summary>
        /// Registers resource API-related services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="resourceHandlerRegistration"></param>
        /// <returns></returns>
        public static IServiceCollection AddFimsResourceApi(this IServiceCollection services, IResourceHandlerRegistration resourceHandlerRegistration)
            => services
               .AddFimsResourceHandling(resourceHandlerRegistration)
               .AddFimsServerDefaultApi()
               .AddScoped<IRequest, HttpRequestMessageRequest>();

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
            => services.AddScoped<IWorker, T>();
    }
}