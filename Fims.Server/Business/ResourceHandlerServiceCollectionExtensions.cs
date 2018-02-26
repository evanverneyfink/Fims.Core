using System;
using Microsoft.Extensions.DependencyInjection;

namespace Fims.Server.Business
{
    public static class ResourceHandlerServiceCollectionExtensions
    {
        /// <summary>
        /// Adds resource handling
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddFimsResourceHandling(this IServiceCollection serviceCollection,
                                                                 Action<ResourceHandlerRegistryOptions> configureOptions = null)
        {
            serviceCollection.AddSingleton(svcProvider =>
                             {
                                 var opts = new ResourceHandlerRegistryOptions(serviceCollection);
                                 configureOptions?.Invoke(opts);
                                 return opts.Configure(svcProvider);
                             })
                             .AddSingleton<IResourceHandlerRegistry, ResourceHandlerRegistry>()
                             .AddScoped(typeof(IResourceHandler<>), typeof(ResourceHandler<>));

            return serviceCollection;
        }
    }
}