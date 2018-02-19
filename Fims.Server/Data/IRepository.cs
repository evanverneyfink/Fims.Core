using System.Collections.Generic;
using System.Threading.Tasks;
using Fims.Core.Model;

namespace Fims.Server.Data
{
    public interface IRepository
    {
        /// <summary>
        /// Gets a resource of type <see cref="T"/> by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> Get<T>(string id) where T : Resource;

        /// <summary>
        /// Queries resources of type <see cref="T"/> using the provided criteria, in the form of key/value pairs
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> Query<T>(IDictionary<string, string> parameters) where T : Resource;

        /// <summary>
        /// Creates a resource of type <see cref="T"/>
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        Task<T> Create<T>(T resource) where T : Resource;

        /// <summary>
        /// Updates a resource of type <see cref="T"/>
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        Task<T> Update<T>(T resource) where T : Resource;

        /// <summary>
        /// Deletes a resource of type <see cref="T"/> by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete<T>(string id) where T : Resource;
    }
}
