using System;
using System.Collections.Generic;
using System.Linq;

namespace Fims.Server.Business
{
    public class ResourceHandlerRegistryOptions
    {
        /// <summary>
        /// Instantiates a <see cref="ResourceHandlerRegistryOptions"/>
        /// </summary>
        public ResourceHandlerRegistryOptions()
        {
        }

        /// <summary>
        /// Instantiates a <see cref="ResourceHandlerRegistryOptions"/>
        /// </summary>
        /// <param name="handlers"></param>
        public ResourceHandlerRegistryOptions(params ValueTuple<Type, Func<IResourceHandler>>[] handlers)
        {
            RegisteredHandlers = handlers.ToDictionary(x => x.Item1, x => x.Item2);
        }

        /// <summary>
        /// Registers a resource handler
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="createHandler"></param>
        /// <returns></returns>
        public ResourceHandlerRegistryOptions Register<T>(Func<IResourceHandler> createHandler) where T : IResourceHandler
        {
            RegisteredHandlers[typeof(T)] = createHandler;
            return this;
        }

        /// <summary>
        /// Gets the registered resource handlers
        /// </summary>
        internal IDictionary<Type, Func<IResourceHandler>> RegisteredHandlers { get; } = new Dictionary<Type, Func<IResourceHandler>>();
        
        /// <summary>
        /// Delegate for getting the default hander
        /// </summary>
        internal Func<Type, IResourceHandler> GetDefaultHandler { get; set; }
    }
}