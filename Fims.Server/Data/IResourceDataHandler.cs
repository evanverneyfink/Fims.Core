using System.Collections.Generic;
using System.Threading.Tasks;
using Fims.Core.Model;

namespace Fims.Server.Data
{
    public interface IResourceDataHandler
    {
        /// <summary>
        /// Gets a resource by its ID
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <returns></returns>
        Task<T> Get<T>(ResourceDescriptor resourceDescriptor) where T : Resource;

        /// <summary>
        /// Queries resources using the provided criteria, in the form of key/value pairs
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> Query<T>(ResourceDescriptor resourceDescriptor) where T : Resource;

        /// <summary>
        /// Creates a resource
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        Task<T> Create<T>(ResourceDescriptor resourceDescriptor, T resource) where T : Resource;

        /// <summary>
        /// Updates a resource
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        Task<T> Update<T>(ResourceDescriptor resourceDescriptor, T resource) where T : Resource;

        /// <summary>
        /// Deletes a resource by its ID
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <returns></returns>
        Task Delete<T>(ResourceDescriptor resourceDescriptor) where T : Resource;
    }
}