﻿using System;
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
            // create registry options for service collection
            var opts = new ResourceHandlerRegistryOptions(serviceCollection);

            // apply any configuration
            configureOptions?.Invoke(opts);

            return
                serviceCollection.AddScoped<IResourceHandlerRegistry, ResourceHandlerRegistry>()
                                 .AddScoped(svcProvider => opts.Configure(svcProvider));
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

        /// <summary>
        /// Adds resource handling
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="resourceHandlerRegistration"></param>
        /// <returns></returns>
        public static IServiceCollection AddFimsResourceHandling(this IServiceCollection serviceCollection,
                                                                 IResourceHandlerRegistration resourceHandlerRegistration)
        {
            return serviceCollection.AddFimsResourceHandling(resourceHandlerRegistration != null
                                                                 ? resourceHandlerRegistration.Register
                                                                 : default(Action<ResourceHandlerRegistryOptions>));
        }
    }
}