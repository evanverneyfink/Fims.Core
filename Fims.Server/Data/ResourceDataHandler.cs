using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fims.Core.Model;

namespace Fims.Server.Data
{
    public class ResourceDataHandler<T> : IResourceDataHandler<T> where T : Resource
    {
        /// <summary>
        /// Instantiates a <see cref="ResourceDataHandler{T}"/>
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="environment"></param>
        /// <param name="repositoryHandler"></param>
        /// <param name="httpHandler"></param>
        public ResourceDataHandler(ILogger logger, IEnvironment environment, IRepositoryResourceDataHandler<T> repositoryHandler, IHttpResourceDataHandler<T> httpHandler)
        {
            Logger = logger;
            Environment = environment;
            RepositoryHandler = repositoryHandler;
            HttpHandler = httpHandler;
        }

        /// <summary>
        /// Gets the logger
        /// </summary>
        private ILogger Logger { get; }

        /// <summary>
        /// Gets the environment
        /// </summary>
        private IEnvironment Environment { get; }

        /// <summary>
        /// Gets the data handler that uses the local service's repository
        /// </summary>
        private IRepositoryResourceDataHandler<T> RepositoryHandler { get; }

        /// <summary>
        /// Gets the data handler that uses HTTP calls
        /// </summary>
        private IHttpResourceDataHandler<T> HttpHandler { get; }

        /// <summary>
        /// Executes an action against either the repository or HTTP handler based on the resource descriptor
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <param name="execute"></param>
        /// <returns></returns>
        private Task Execute(ResourceDescriptor resourceDescriptor, Func<IResourceDataHandler<T>, Task> execute)
        {
            var isLocal = resourceDescriptor.Url.StartsWith(Environment.PublicUrl);

            Logger.Debug("Executing operation for {0} resource at {1}", isLocal ? "local" : "remote", resourceDescriptor.Url);

            return isLocal ? execute(RepositoryHandler) : execute(HttpHandler);
        }

        /// <summary>
        /// Executes an action against either the repository or HTTP handler based on the resource descriptor
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <param name="execute"></param>
        /// <returns></returns>
        private Task<TResult> Execute<TResult>(ResourceDescriptor resourceDescriptor, Func<IResourceDataHandler<T>, Task<TResult>> execute)
        {
            var isLocal = resourceDescriptor.Url.StartsWith(Environment.PublicUrl);

            Logger.Debug("Executing operation for {0} resource at {1}", isLocal ? "local" : "remote", resourceDescriptor.Url);

            return isLocal ? execute(RepositoryHandler) : execute(HttpHandler);
        }

        /// <summary>
        /// Gets a resource of type <see cref="T"/> by its ID
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public Task<T> Create(ResourceDescriptor resourceDescriptor, T resource)
        {
            return Execute(resourceDescriptor, handler => handler.Create(resourceDescriptor, resource));
        }

        /// <summary>
        /// Deletes a resource of type <see cref="T"/> by its ID
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <returns></returns>
        public Task Delete(ResourceDescriptor resourceDescriptor)
        {
            return Execute(resourceDescriptor, handler => handler.Delete(resourceDescriptor));
        }

        /// <summary>
        /// Gets a resource of type <see cref="T"/> by its ID
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <returns></returns>
        public Task<T> Get(ResourceDescriptor resourceDescriptor)
        {
            return Execute(resourceDescriptor, handler => handler.Get(resourceDescriptor));
        }

        /// <summary>
        /// Queries resources of type <see cref="T"/> using the provided criteria, in the form of key/value pairs
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <returns></returns>
        public Task<IEnumerable<T>> Query(ResourceDescriptor resourceDescriptor)
        {
            Logger.Debug("Querying resources of type {0}...", resourceDescriptor.Type.Name);

            return Execute(resourceDescriptor, handler => handler.Query(resourceDescriptor));
        }

        /// <summary>
        /// Gets a resource of type <see cref="T"/> by its ID
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public Task<T> Update(ResourceDescriptor resourceDescriptor, T resource)
        {
            return Execute(resourceDescriptor, handler => handler.Update(resourceDescriptor, resource));
        }
    }
}