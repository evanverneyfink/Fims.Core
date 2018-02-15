using System;
using System.Collections.Generic;

namespace Fims.Server.Business
{
    public interface IResourceHandlerRegistry
    {
        /// <summary>
        /// Gets the registered resource handlers
        /// </summary>
        IDictionary<Type, Func<IResourceHandler>> RegisteredHandlers { get; }

        /// <summary>
        /// Delegate for getting the default hander
        /// </summary>
        Func<Type, IResourceHandler> GetDefaultHandler { get; }
    }
}