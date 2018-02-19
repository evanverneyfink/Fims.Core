using System.Threading.Tasks;
using Fims.Core.Model;

namespace Fims.Server.Data
{
    public static class ResourceDataHandlerExtensions
    {
        /// <summary>
        /// Gets a resource by its url
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resourceDataHandler"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Task<T> Get<T>(this IResourceDataHandler resourceDataHandler, string url) where T : Resource
        {
            return resourceDataHandler.Get<T>(ResourceDescriptor.FromUrl<T>(url));
        }

        /// <summary>
        /// Updates a resource
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resourceDataHandler"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public static Task<T> Update<T>(this IResourceDataHandler resourceDataHandler, T resource) where T : Resource
        {
            return resourceDataHandler.Update<T>(ResourceDescriptor.FromUrl<T>(resource.Id), resource);
        }
    }
}