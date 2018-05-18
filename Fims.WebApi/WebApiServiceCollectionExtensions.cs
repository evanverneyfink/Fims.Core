using Fims.Server.Api;
using Fims.Server.Business;
using Fims.Server.Data;
using Fims.Server.Environment;
using Fims.Services.Jobs.WorkerFunctions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fims.WebApi
{
    public static class WebApiServiceCollectionExtensions
    {
        public static IServiceCollection AddFimsWebApi<T>(this IServiceCollection services, IConfiguration configuration)
            where T : IResourceHandlerRegistration, new()
        {
            return services.AddEnvironment(opts => opts.AddIisExpressUrl().AddProvider(new WebApiConfigValueProvider(configuration)))
                           .AddFimsResourceDataHandling()
                           .AddFimsResourceHandling<T>()
                           .AddFimsServerDefaultApi()
                           .AddScoped<IRequest, WebApiRequest>();
        }

        public static IServiceCollection AddFimsWorkerFunctionWebApi<T>(this IServiceCollection services, IConfiguration configuration)
            where T : class, IWorkerFunctionInvoker
        {
            return services.AddEnvironment(opts => opts.AddIisExpressUrl().AddProvider(new WebApiConfigValueProvider(configuration)))
                           .AddFimsResourceDataHandling()
                           .AddFimsWorkerFunctionApi<T>()
                           .AddFimsServerDefaultApi()
                           .AddScoped<IRequest, WebApiRequest>();
        }

        public static IServiceCollection AddFimsInProcessWorkerFunctionWebApi<T>(this IServiceCollection services, IConfiguration configuration)
            where T : class, IWorker
        {
            return services.AddEnvironment(opts => opts.AddIisExpressUrl().AddProvider(new WebApiConfigValueProvider(configuration)))
                           .AddFimsResourceDataHandling()
                           .AddFimsInProcesssWorkerFunctionApi<T>()
                           .AddFimsServerDefaultApi()
                           .AddScoped<IRequest, WebApiRequest>();
        }
    }
}