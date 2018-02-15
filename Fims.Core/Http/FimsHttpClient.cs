using System.Net.Http;
using System.Threading.Tasks;
using Fims.Core.JsonLd;
using Newtonsoft.Json.Linq;

namespace Fims.Core.Http
{
    public class FimsHttpClient : IFimsHttpClient
    {
        /// <summary>
        /// Instantiates a <see cref="FimsHttpClient"/>
        /// </summary>
        /// <param name="jsonLdContextManager"></param>
        /// <param name="jsonLdProcessor"></param>
        public FimsHttpClient(IJsonLdContextManager jsonLdContextManager, IJsonLdProcessor jsonLdProcessor)
        {
            JsonLdContextManager = jsonLdContextManager;
            JsonLdProcessor = jsonLdProcessor;
        }

        /// <summary>
        /// Gets the JSON-LD context manager
        /// </summary>
        private IJsonLdContextManager JsonLdContextManager { get; }

        /// <summary>
        /// Gets the JSON-LD helper
        /// </summary>
        private IJsonLdProcessor JsonLdProcessor { get; }

        /// <summary>
        /// Gets the underlying http client
        /// </summary>
        private HttpClient HttpClient { get; } = new HttpClient();

        /// <summary>
        /// Gets a FIMS resource
        /// </summary>
        /// <param name="url"></param>
        /// <param name="contextUrl"></param>
        /// <returns></returns>
        public Task<JToken> GetAsync(string url, string contextUrl = null)
        {
            return SendAsync(HttpMethod.Get, url, contextUrl: contextUrl);
        }

        /// <summary>
        /// Posts a FIMS resource
        /// </summary>
        /// <param name="url"></param>
        /// <param name="obj"></param>
        /// <param name="contextUrl"></param>
        /// <returns></returns>
        public Task<JToken> PostAsync(string url, object obj, string contextUrl = null)
        {
            return SendAsync(HttpMethod.Post, url, obj, contextUrl);
        }

        /// <summary>
        /// Puts a FIMS resource
        /// </summary>
        /// <param name="url"></param>
        /// <param name="obj"></param>
        /// <param name="contextUrl"></param>
        /// <returns></returns>
        public Task<JToken> PutAsync(string url, object obj, string contextUrl)
        {
            return SendAsync(HttpMethod.Put, url, obj, contextUrl);
        }

        /// <summary>
        /// Deletes a FIMS resource
        /// </summary>
        /// <param name="url"></param>
        /// <param name="contextUrl"></param>
        /// <returns></returns>
        public Task<JToken> DeleteAsync(string url, string contextUrl)
        {
            return SendAsync(HttpMethod.Delete, url, contextUrl: contextUrl);
        }

        /// <summary>
        /// Sends a FIMS HTTP request
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <param name="contextUrl"></param>
        /// <returns></returns>
        private async Task<JToken> SendAsync(HttpMethod method, string url, object body = null, string contextUrl = null)
        {
            // use default context if not specified
            contextUrl = contextUrl ?? JsonLdContextManager.DefaultUrl;

            // create request
            var request = new HttpRequestMessage(method, url);

            // set the body of the request if provided
            if (body != null)
                request.Content = new JsonContent(body);

            // send the request and ensure it does not return an error code
            var resp = (await HttpClient.SendAsync(request)).EnsureSuccessStatusCode();

            // read the content of the response as FIMS json
            return await resp.Content.ReadAsJsonAsync(HttpMethod.Put, url, async i => await JsonLdProcessor.Compact(i, contextUrl));
        }
    }
}