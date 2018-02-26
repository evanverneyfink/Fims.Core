using System;

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
        /// Checks if a resource type is supported
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsSupported(Type type) => Options.SupportedTypes.Contains(type);

        /// <summary>
        /// Delegate for getting the default hander
        /// </summary>
        public IResourceHandler Get(Type type)
        {
            return Options.CreateHandler(type);
        }
    }
}