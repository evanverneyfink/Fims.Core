using Fims.Core;
using Fims.Server;
using Fims.Server.Api;
using Fims.Server.Business;
using Fims.Server.Data;
using Fims.Services.Jobs.WorkerFunctions;
using Microsoft.Extensions.DependencyInjection;

namespace Fims.WebApi
{
    public static class WebApiServiceCollectionExtensions
    {
        public static IServiceCollection AddFimsWebApi<T>(this IServiceCollection services)
            where T : IResourceHandlerRegistration, new()
        {
            return services.AddFimsResourceDataHandling()
                           .AddFimsResourceHandling<T>()
                           .AddFimsServerDefaultApi()
                           .AddSingleton<IEnvironment, WebApiEnvironment>()
                           .AddScoped<IRequestContext, WebApiRequestContext>();
        }

        public static IServiceCollection AddFimsWorkerFunctionWebApi<T>(this IServiceCollection services) where T : class, IWorkerFunctionInvoker
        {
            return services.AddFimsResourceDataHandling()
                           .AddFimsWorkerFunctionApi<T>()
                           .AddFimsServerDefaultApi()
                           .AddSingleton<IEnvironment, WebApiEnvironment>()
                           .AddScoped<IRequestContext, WebApiRequestContext>();
        }

        public static IServiceCollection AddFimsInProcessWorkerFunctionWebApi<T>(this IServiceCollection services) where T : class, IWorker
        {
            return services.AddFimsResourceDataHandling()
                           .AddFimsInProcesssWorkerFunctionApi<T>()
                           .AddFimsServerDefaultApi()
                           .AddSingleton<IEnvironment, WebApiEnvironment>()
                           .AddScoped<IRequestContext, WebApiRequestContext>();
        }
    }
}