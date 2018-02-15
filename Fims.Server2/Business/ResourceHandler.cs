using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fims.Core.Model;
using Fims.Server.Data;

namespace Fims.Server.Business
{
    public class ResourceHandler<T> : IResourceHandler, IResourceHandler<T> where T : Resource
    {
        /// <summary>
        /// Instantiates a <see cref="ResourceHandler{T}"/>
        /// </summary>
        /// <param name="dataHandler"></param>
        public ResourceHandler(ILogger logger, IResourceDataHandler<T> dataHandler)
        {
            Logger = logger;
            DataHandler = dataHandler;
        }

        /// <summary>
        /// Gets the logger
        /// </summary>
        private ILogger Logger { get; }

        /// <summary>
        /// Gets the repository
        /// </summary>
        private IResourceDataHandler<T> DataHandler { get; }

        #region Explicit IResourceHandler Implementation

        /// <summary>
        /// Gets a resource by its ID
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <returns></returns>
        async Task<Resource> IResourceHandler.Get(ResourceDescriptor resourceDescriptor)
        {
            Logger.Debug("IResourceHandler.Get");

            return await Get(resourceDescriptor);
        }

        /// <summary>
        /// Queries resources using the provided criteria, in the form of key/value pairs
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <returns></returns>
        async Task<IEnumerable<Resource>> IResourceHandler.Query(ResourceDescriptor resourceDescriptor)
        {
            return await Query(resourceDescriptor);
        }

        /// <summary>
        /// Creates a resource
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        async Task<Resource> IResourceHandler.Create(ResourceDescriptor resourceDescriptor, Resource resource)
        {
            return await Create(resourceDescriptor, (T)resource);
        }

        /// <summary>
        /// Updates a resource
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        async Task<Resource> IResourceHandler.Update(ResourceDescriptor resourceDescriptor, Resource resource)
        {
            return await Update(resourceDescriptor, (T)resource);
        }

        /// <summary>
        /// Deletes a resource by its ID
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <returns></returns>
        async Task IResourceHandler.Delete(ResourceDescriptor resourceDescriptor)
        {
            await Delete(resourceDescriptor);
        }

        #endregion

        /// <summary>
        /// Gets a resource of type <see cref="T"/> by its ID
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <returns></returns>
        public virtual async Task<T> Get(ResourceDescriptor resourceDescriptor)
        {
            return await DataHandler.Get(resourceDescriptor);
        }

        /// <summary>
        /// Queries resources of type <see cref="T"/> using the provided criteria, in the form of key/value pairs
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> Query(ResourceDescriptor resourceDescriptor)
        {
            return await DataHandler.Query(resourceDescriptor);
        }

        /// <summary>
        /// Gets a resource of type <see cref="T"/> by its ID
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public virtual async Task<T> Create(ResourceDescriptor resourceDescriptor, T resource)
        {
            resource.Id = resourceDescriptor.Id = Guid.NewGuid().ToString();

            if (!resource.Created.HasValue)
                resource.Created = DateTime.UtcNow;
            if (!resource.Modified.HasValue)
                resource.Modified = DateTime.UtcNow;

            resource = await DataHandler.Create(resourceDescriptor, resource);

            await OnCreated(resourceDescriptor, resource);

            return resource;
        }

        /// <summary>
        /// Gets a resource of type <see cref="T"/> by its ID
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public virtual async Task<T> Update(ResourceDescriptor resourceDescriptor, T resource)
        {
            resource.Modified = DateTime.UtcNow;

            resource = await DataHandler.Update(resourceDescriptor, resource);

            await OnUpdated(resourceDescriptor, resource);

            return resource;
        }

        /// <summary>
        /// Deletes a resource of type <see cref="T"/> by its ID
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <returns></returns>
        public virtual async Task Delete(ResourceDescriptor resourceDescriptor)
        {
            await DataHandler.Delete(resourceDescriptor);

            await OnDeleted(resourceDescriptor);
        }

        /// <summary>
        /// Handles successful creation of a resource
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <param name="resource"></param>
        protected virtual Task<T> OnCreated(ResourceDescriptor resourceDescriptor, T resource)
        {
            return Task.FromResult(resource);
        }

        /// <summary>
        /// Handle update of a resource
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <param name="resource"></param>
        protected virtual Task<T> OnUpdated(ResourceDescriptor resourceDescriptor, T resource)
        {
            return Task.FromResult(resource);
        }

        /// <summary>
        /// Handle deletion of a resource
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        protected virtual Task OnDeleted(ResourceDescriptor resourceDescriptor)
        {
            return Task.CompletedTask;
        }
    }
}