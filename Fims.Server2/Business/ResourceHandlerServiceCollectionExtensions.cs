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
            serviceCollection.AddSingleton<IResourceHandlerProvider, DefaultResourceHandlerProvider>()
                             .AddSingleton(svcProvider =>
                             {
                                 var opts = new ResourceHandlerRegistryOptions();
                                 configureOptions?.Invoke(opts);
                                 if (opts.GetDefaultHandler == null)
                                     opts.GetDefaultHandler =
                                         t =>
                                         {
                                             var logger = svcProvider.GetService<ILogger>();
                                             logger.Info("Using default resource handler for resource type {0}", t.Name);
                                             var handler = (IResourceHandler)svcProvider.GetService(typeof(IResourceHandler<>).MakeGenericType(t));
                                             if (handler == null)
                                                 logger.Warning(
                                                     "Failed to create default handler for resource type {0}. Service provider was unable to resolve generic type.");
                                             return handler;
                                         };
                                 return opts;
                             })
                             .AddSingleton<IResourceHandlerRegistry, ResourceHandlerRegistry>()
                             .AddScoped(typeof(IResourceHandler<>), typeof(ResourceHandler<>));

            return serviceCollection;
        }
    }
}