using System;
using System.Collections.Generic;

namespace Fims.Server.Business
{
    public class ResourceHandlerRegistry : IResourceHandlerRegistry
    {
        /// <summary>
        /// Instantiates a <see cref="ResourceHandlerRegistry"/>
        /// </summary>
        /// <param name="options"></param>
        public ResourceHandlerRegistry(ResourceHandlerRegistryOptions options)
        {
            Options = options;
        }

        /// <summary>
        /// Gets the options
        /// </summary>
        private ResourceHandlerRegistryOptions Options { get; }

        /// <summary>
        /// Gets the registered resource handlers
        /// </summary>
        public IDictionary<Type, Func<IResourceHandler>> FactoryOverrides => Options.RegisteredHandlers;

        /// <summary>
        /// Delegate for getting the default hander
        /// </summary>
        public Func<Type, IResourceHandler> DefaultFactory => Options.GetDefaultHandler;

        /// <summary>
        /// Checks if a resource type is supported
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsSupported(Type type) => Options.SupportedTypes.Contains(type);
    }
}