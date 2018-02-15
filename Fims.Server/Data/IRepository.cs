using System.Collections.Generic;
using System.Threading.Tasks;
using Fims.Core.Model;

namespace Fims.Server.Data
{
    public interface IRepository<T> where T : Resource
    {
        /// <summary>
        /// Gets a resource of type <see cref="T"/> by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> Get(string id);

        /// <summary>
        /// Queries resources of type <see cref="T"/> using the provided criteria, in the form of key/value pairs
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> Query(IDictionary<string, string> parameters);

        /// <summary>
        /// Creates a resource of type <see cref="T"/>
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        Task<T> Create(T resource);

        /// <summary>
        /// Updates a resource of type <see cref="T"/>
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        Task<T> Update(T resource);

        /// <summary>
        /// Deletes a resource of type <see cref="T"/> by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(string id);
    }
}
