using System;

namespace Fims.Server.Business
{
    public interface IResourceHandlerProvider
    {
        /// <summary>
        /// Checks if a resource type is supported by this request processor
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool IsSupportedResourceType(Type type);

        /// <summary>
        /// Creates a resource handler
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <returns></returns>
        IResourceHandler CreateResourceHandler(ResourceDescriptor resourceDescriptor);
    }
}