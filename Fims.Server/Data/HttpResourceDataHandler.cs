using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Fims.Core.Http;
using Fims.Core.JsonLd;
using Fims.Core.Model;

namespace Fims.Server.Data
{
    public class HttpResourceDataHandler<T> : IHttpResourceDataHandler<T> where T : Resource
    {
        /// <summary>
        /// Instantiates a <see cref="HttpResourceDataHandler{T}"/>
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="jsonLdResourceHelper"></param>
        public HttpResourceDataHandler(IEnvironment environment, IJsonLdResourceHelper jsonLdResourceHelper)
        {
            Environment = environment;
            JsonLdResourceHelper = jsonLdResourceHelper;
        }

        /// <summary>
        /// Gets the environment
        /// </summary>
        private IEnvironment Environment { get; }

        /// <summary>
        /// Gets the JSON LD resource helper
        /// </summary>
        private IJsonLdResourceHelper JsonLdResourceHelper { get; }

        /// <summary>
        /// Gets the repository
        /// </summary>
        private HttpClient HttpClient { get; } = new HttpClient();

        /// <summary>
        /// Gets a resource of type <see cref="T"/> by its ID
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <returns></returns>
        public async Task<T> Get(ResourceDescriptor resourceDescriptor)
        {
            var resp = await HttpClient.GetAsync(resourceDescriptor.Url);

            resp.EnsureSuccessStatusCode();

            return await resp.Content.ReadAsResourceAsync(
                       HttpMethod.Get,
                       resourceDescriptor.Url,
                       async j => (T)(await JsonLdResourceHelper.GetResourceFromJson(j, typeof(T), Environment.PublicUrl)));
        }

        /// <summary>
        /// Queries resources of type <see cref="T"/> using the provided criteria, in the form of key/value pairs
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> Query(ResourceDescriptor resourceDescriptor)
        {
            var resp = await HttpClient.GetAsync(resourceDescriptor.Url);

            resp.EnsureSuccessStatusCode();

            return await resp.Content.ReadAsResourceCollectionAsync(
                       HttpMethod.Get,
                       resourceDescriptor.Url,
                       async j => (T)await JsonLdResourceHelper.GetResourceFromJson(j, typeof(T), Environment.PublicUrl));
        }

        /// <summary>
        /// Gets a resource of type <see cref="T"/> by its ID
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public virtual async Task<T> Create(ResourceDescriptor resourceDescriptor, T resource)
        {
            var resp = await HttpClient.PostAsync(resourceDescriptor.Url,
                                                  new JsonContent(JsonLdResourceHelper.GetJsonFromResource(resource, Environment.PublicUrl)));

            resp.EnsureSuccessStatusCode();

            return await resp.Content.ReadAsResourceAsync(
                       HttpMethod.Post,
                       resourceDescriptor.Url,
                       async j => (T)await JsonLdResourceHelper.GetResourceFromJson(j, typeof(T), Environment.PublicUrl));
        }

        /// <summary>
        /// Gets a resource of type <see cref="T"/> by its ID
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public virtual async Task<T> Update(ResourceDescriptor resourceDescriptor, T resource)
        {
            var resp = await HttpClient.PutAsync(resourceDescriptor.Url,
                                                 new JsonContent(JsonLdResourceHelper.GetJsonFromResource(resource, Environment.PublicUrl)));

            resp.EnsureSuccessStatusCode();

            return await resp.Content.ReadAsResourceAsync(
                       HttpMethod.Put,
                       resourceDescriptor.Url,
                       async j => (T)await JsonLdResourceHelper.GetResourceFromJson(j, typeof(T), Environment.PublicUrl));
        }

        /// <summary>
        /// Deletes a resource of type <see cref="T"/> by its ID
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <returns></returns>
        public virtual async Task Delete(ResourceDescriptor resourceDescriptor)
        {
            (await HttpClient.DeleteAsync(resourceDescriptor.Url)).EnsureSuccessStatusCode();
        }
    }
}