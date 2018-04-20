using System;
using System.Collections.Generic;
using Fims.Core.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Fims.Server.Business
{
    public class ResourceHandlerRegistryOptions
    {
        /// <summary>
        /// Instantiates a <see cref="ResourceHandlerRegistryOptions"/>
        /// </summary>
        /// <param name="serviceCollection"></param>
        public ResourceHandlerRegistryOptions(IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection;
        }

        /// <summary>
        /// Gets the service collection
        /// </summary>
        private IServiceCollection ServiceCollection { get; }

        /// <summary>
        /// Gets the collection of supported types
        /// </summary>
        internal List<Type> SupportedTypes { get; } = new List<Type>();
        
        /// <summary>
        /// Delegate for getting the default hander
        /// </summary>
        internal Func<Type, IResourceHandler> CreateHandler { get; set; }

        /// <summary>
        /// Registers a resource handler
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="createHandler"></param>
        /// <param name="serviceLifetime"></param>
        /// <returns></returns>
        public ResourceHandlerRegistryOptions Register<T>(Func<IServiceProvider, IResourceHandler<T>> createHandler = null,
                                                          ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where T : Resource, new()
        {
            SupportedTypes.Add(typeof(T));
            if (createHandler != null)
                ServiceCollection.Add(new ServiceDescriptor(typeof(IResourceHandler<T>), createHandler, serviceLifetime));
            else
                ServiceCollection.Add(new ServiceDescriptor(typeof(IResourceHandler<T>), typeof(ResourceHandler<T>), serviceLifetime));
            return this;
        }

        /// <summary>
        /// Registers a resource handler
        /// </summary>
        /// <typeparam name="TResource"></typeparam>
        /// <typeparam name="THandler"></typeparam>
        /// <param name="serviceLifetime"></param>
        /// <returns></returns>
        public ResourceHandlerRegistryOptions Register<TResource, THandler>(ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TResource : Resource, new()
            where THandler : IResourceHandler<TResource>
        {
            SupportedTypes.Add(typeof(TResource));
            ServiceCollection.Add(new ServiceDescriptor(typeof(IResourceHandler<TResource>), typeof(THandler), serviceLifetime));
            return this;
        }

        /// <summary>
        /// Configures the options with the given service provider
        /// </summary>
        /// <returns></returns>
        internal ResourceHandlerRegistryOptions Configure(IServiceProvider svcProvider)
        {
            if (CreateHandler == null)
                CreateHandler =
                    t =>
                    {
                        var logger = svcProvider.GetService<ILogger>();
                        logger.Info("Using default resource handler for resource type {0}", t.Name);

                        var handlerType = typeof(IResourceHandler<>).MakeGenericType(t);

                        var handler = (IResourceHandler)svcProvider.GetService(handlerType);
                        if (handler == null)
                            logger.Warning(
                                "Failed to create default handler for resource type {0}. Service provider was unable to resolve generic type.", t.Name);

                        return handler;
                    };

            return this;
        }
    }
}