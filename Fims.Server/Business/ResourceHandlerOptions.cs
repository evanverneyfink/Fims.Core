using System;
using System.Collections.Generic;

namespace Fims.Server.Business
{
    public interface IResourceHandlerRegistry
    {
        /// <summary>
        /// Gets the registered resource handlers
        /// </summary>
        IDictionary<Type, Func<IResourceHandler>> FactoryOverrides { get; }

        /// <summary>
        /// Delegate for getting the default hander
        /// </summary>
        Func<Type, IResourceHandler> DefaultFactory { get; }

        /// <summary>
        /// Checks if a resource type is supported
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool IsSupported(Type type);
    }
}