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
        public IDictionary<Type, Func<IResourceHandler>> RegisteredHandlers => Options.RegisteredHandlers;

        /// <summary>
        /// Delegate for getting the default hander
        /// </summary>
        public Func<Type, IResourceHandler> GetDefaultHandler => Options.GetDefaultHandler;
    }
}