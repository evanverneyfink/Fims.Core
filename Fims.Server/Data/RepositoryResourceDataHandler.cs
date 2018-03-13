using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fims.Core;
using Fims.Core.Model;

namespace Fims.Server.Data
{
    public class RepositoryResourceDataHandler : IRepositoryResourceDataHandler
    {
        /// <summary>
        /// Instantiates a <see cref="RepositoryResourceDataHandler"/>
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="documentHelper"></param>
        public RepositoryResourceDataHandler(IRepository repository, IDocumentHelper documentHelper)
        {
            Repository = repository;
            DocumentHelper = documentHelper;
        }

        /// <summary>
        /// Gets the repository
        /// </summary>
        private IRepository Repository { get; }

        /// <summary>
        /// Gets the document helper
        /// </summary>
        private IDocumentHelper DocumentHelper { get; }

        /// <summary>
        /// Gets a resource by its ID
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <returns></returns>
        public async Task<Resource> Get(ResourceDescriptor resourceDescriptor)
        {
            var resource = await Repository.Get(resourceDescriptor.Type, resourceDescriptor.Url);

            return DocumentHelper.GetResource(resourceDescriptor.Type, resource);
        }

        /// <summary>
        /// Gets a resource of type <see cref="T"/> by its ID
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <returns></returns>
        public async Task<T> Get<T>(ResourceDescriptor resourceDescriptor) where T : Resource, new()
        {
            var resource = await Repository.Get<T>(resourceDescriptor.Url);

            return DocumentHelper.GetResource<T>(resource);
        }

        /// <summary>
        /// Queries resources of type <see cref="T"/> using the provided criteria, in the form of key/value pairs
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> Query<T>(ResourceDescriptor resourceDescriptor) where T : Resource, new()
        {
            var resources = await Repository.Query<T>(resourceDescriptor.Parameters);

            return resources.Select<dynamic, T>(r => DocumentHelper.GetResource<T>(r));
        }

        /// <summary>
        /// Creates a resource
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public async Task<Resource> Create(ResourceDescriptor resourceDescriptor, Resource resource)
        {
            var newResource = await Repository.Create(resourceDescriptor.Type, DocumentHelper.GetDocument(resource));

            return DocumentHelper.GetResource(resourceDescriptor.Type, newResource);
        }

        /// <summary>
        /// Gets a resource of type <see cref="T"/> by its ID
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public async Task<T> Create<T>(ResourceDescriptor resourceDescriptor, T resource) where T : Resource, new()
        {
            var newResource = await Repository.Create<T>(DocumentHelper.GetDocument(resource));

            return DocumentHelper.GetResource<T>(newResource);
        }

        /// <summary>
        /// Updates a resource
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public async Task<Resource> Update(ResourceDescriptor resourceDescriptor, Resource resource)
        {
            var newResource = await Repository.Update(resourceDescriptor.Type, DocumentHelper.GetDocument(resource));

            return DocumentHelper.GetResource(resourceDescriptor.Type, newResource);
        }

        /// <summary>
        /// Gets a resource of type <see cref="T"/> by its ID
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public async Task<T> Update<T>(ResourceDescriptor resourceDescriptor, T resource) where T : Resource, new()
        {
            var newResource = await Repository.Update<T>(DocumentHelper.GetDocument(resource));

            return DocumentHelper.GetResource<T>(newResource);
        }

        /// <summary>
        /// Deletes a resource by its ID
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <returns></returns>
        public Task Delete<T>(ResourceDescriptor resourceDescriptor) where T : Resource, new()
        {
            return Repository.Delete<T>(resourceDescriptor.Id);
        }
    }
}