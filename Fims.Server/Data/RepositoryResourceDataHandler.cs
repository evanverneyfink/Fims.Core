using System.Collections.Generic;
using System.Threading.Tasks;
using Fims.Core.Model;

namespace Fims.Server.Data
{
    public class RepositoryResourceDataHandler : IRepositoryResourceDataHandler
    {
        /// <summary>
        /// Instantiates a <see cref="RepositoryResourceDataHandler"/>
        /// </summary>
        /// <param name="repository"></param>
        public RepositoryResourceDataHandler(IRepository repository)
        {
            Repository = repository;
        }

        /// <summary>
        /// Gets the repository
        /// </summary>
        private IRepository Repository { get; }

        /// <summary>
        /// Gets a resource of type <see cref="T"/> by its ID
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <returns></returns>
        public Task<T> Get<T>(ResourceDescriptor resourceDescriptor) where T : Resource
        {
            return Repository.Get<T>(resourceDescriptor.Url);
        }

        /// <summary>
        /// Queries resources of type <see cref="T"/> using the provided criteria, in the form of key/value pairs
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <returns></returns>
        public Task<IEnumerable<T>> Query<T>(ResourceDescriptor resourceDescriptor) where T : Resource
        {
            return Repository.Query<T>(resourceDescriptor.Parameters);
        }

        /// <summary>
        /// Gets a resource of type <see cref="T"/> by its ID
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public Task<T> Create<T>(ResourceDescriptor resourceDescriptor, T resource) where T : Resource
        {
            return Repository.Create(resource);
        }

        /// <summary>
        /// Gets a resource of type <see cref="T"/> by its ID
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public Task<T> Update<T>(ResourceDescriptor resourceDescriptor, T resource) where T : Resource
        {
            return Repository.Update(resource);
        }

        /// <summary>
        /// Deletes a resource by its ID
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <returns></returns>
        public Task Delete<T>(ResourceDescriptor resourceDescriptor) where T : Resource
        {
            return Repository.Delete<T>(resourceDescriptor.Id);
        }
    }
}