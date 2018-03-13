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
            return
                serviceCollection.AddScoped(svcProvider =>
                                 {
                                     var opts = new ResourceHandlerRegistryOptions(serviceCollection);
                                     configureOptions?.Invoke(opts);
                                     return opts.Configure(svcProvider);
                                 })
                                 .AddScoped<IResourceHandlerRegistry, ResourceHandlerRegistry>()
                                 .AddScoped(typeof(IResourceHandler<>), typeof(ResourceHandler<>));
        }

        /// <summary>
        /// Adds resource handling
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddFimsResourceHandling<T>(this IServiceCollection serviceCollection) where T : IResourceHandlerRegistration, new()
        {
            return serviceCollection.AddFimsResourceHandling(opts => new T().Register(opts));
        }
    }
}