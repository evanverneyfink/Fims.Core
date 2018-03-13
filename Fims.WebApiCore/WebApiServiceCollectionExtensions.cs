using Fims.Server;
using Fims.Server.Api;
using Fims.Server.Business;
using Fims.Server.Data;
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
    }
}