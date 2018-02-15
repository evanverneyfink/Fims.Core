using System.Collections.Generic;
using System.Threading.Tasks;
using Fims.Core.Model;

namespace Fims.Server.Data
{
    public class RepositoryResourceDataHandler<T> : IRepositoryResourceDataHandler<T> where T : Resource
    {
        /// <summary>
        /// Instantiates a <see cref="RepositoryResourceDataHandler{T}"/>
        /// </summary>
        /// <param name="repository"></param>
        public RepositoryResourceDataHandler(IRepository<T> repository)
        {
            Repository = repository;
        }

        /// <summary>
        /// Gets the repository
        /// </summary>
        private IRepository<T> Repository { get; }

        /// <summary>
        /// Gets a resource of type <see cref="T"/> by its ID
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <returns></returns>
        public Task<T> Get(ResourceDescriptor resourceDescriptor)
        {
            return Repository.Get(resourceDescriptor.Id);
        }

        /// <summary>
        /// Queries resources of type <see cref="T"/> using the provided criteria, in the form of key/value pairs
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <returns></returns>
        public Task<IEnumerable<T>> Query(ResourceDescriptor resourceDescriptor)
        {
            return Repository.Query(resourceDescriptor.Parameters);
        }

        /// <summary>
        /// Gets a resource of type <see cref="T"/> by its ID
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public Task<T> Create(ResourceDescriptor resourceDescriptor, T resource)
        {
            return Repository.Create(resource);
        }

        /// <summary>
        /// Gets a resource of type <see cref="T"/> by its ID
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public Task<T> Update(ResourceDescriptor resourceDescriptor, T resource)
        {
            return Repository.Update(resource);
        }

        /// <summary>
        /// Deletes a resource of type <see cref="T"/> by its ID
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <returns></returns>
        public Task Delete(ResourceDescriptor resourceDescriptor)
        {
            return Repository.Delete(resourceDescriptor.Id);
        }
    }
}