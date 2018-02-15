using Fims.Core.Model;

namespace Fims.Server.Business
{
    public static class ResourceHandlerProviderExtensions
    {
        /// <summary>
        /// Creates a resource handler for a remote url
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resourceHandlerProvider"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static IResourceHandler<T> CreateRemoteResourceHandler<T>(this IResourceHandlerProvider resourceHandlerProvider, string url)
            where T : Resource
        {
            // get resource descriptor from url
            var resourceDescriptor = ResourceDescriptor.FromUrl<T>(url);

            // create resource handler from remote url
            return (IResourceHandler<T>)resourceHandlerProvider.CreateResourceHandler(resourceDescriptor);
        }
    }
}