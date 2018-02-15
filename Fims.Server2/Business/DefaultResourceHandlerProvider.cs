﻿using System;

namespace Fims.Server.Business
{
    public class DefaultResourceHandlerProvider : IResourceHandlerProvider
    {
        /// <summary>
        /// Instantiates a <see cref="DefaultResourceHandlerProvider"/>
        /// </summary>
        /// <param name="handlerRegistry"></param>
        public DefaultResourceHandlerProvider(IResourceHandlerRegistry handlerRegistry)
        {
            HandlerRegistry = handlerRegistry;
        }

        /// <summary>
        /// Gets the resource handler options
        /// </summary>
        private IResourceHandlerRegistry HandlerRegistry { get; }

        /// <summary>
        /// Checks if a resource type is supported by this request processor
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsSupportedResourceType(Type type)
        {
            return true;
        }

        /// <summary>
        /// Creates a resource handler
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <returns></returns>
        public IResourceHandler CreateResourceHandler(ResourceDescriptor resourceDescriptor)
        {
            return HandlerRegistry.RegisteredHandlers.ContainsKey(resourceDescriptor.Type)
                       ? HandlerRegistry.RegisteredHandlers[resourceDescriptor.Type]()
                       : (HandlerRegistry.GetDefaultHandler?.Invoke(resourceDescriptor.Type) ??
                          throw new Exception($"Resource handler for type {resourceDescriptor.Type} not registered."));
        }
    }
}