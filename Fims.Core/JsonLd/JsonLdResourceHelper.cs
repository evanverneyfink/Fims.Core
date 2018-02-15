using System;
using System.Threading.Tasks;
using Fims.Core.Model;
using Newtonsoft.Json.Linq;

namespace Fims.Core.JsonLd
{
    public class JsonLdResourceHelper : IJsonLdResourceHelper
    {
        public JsonLdResourceHelper(IJsonLdContextManager jsonLdContextManager, IJsonLdProcessor jsonLdProcessor)
        {
            JsonLdContextManager = jsonLdContextManager;
            JsonLdProcessor = jsonLdProcessor;
        }

        /// <summary>
        /// Gets the JSON LD context manager
        /// </summary>
        private IJsonLdContextManager JsonLdContextManager { get; }

        /// <summary>
        /// Gets the JSON LD processor
        /// </summary>
        private IJsonLdProcessor JsonLdProcessor { get; }

        /// <summary>
        /// Gets a resource from the JSON in the body of the request
        /// </summary>
        /// <param name="json"></param>
        /// <param name="type"></param>
        /// <param name="contextBaseUrl"></param>
        /// <returns></returns>
        public async Task<Resource> GetResourceFromJson(JToken json, Type type, string contextBaseUrl)
        {
            // use default context
            var contextUrl = contextBaseUrl + "/context/default";

            // ensure default url is mapped to default context
            JsonLdContextManager.Set(contextUrl, JsonLdContextManager.GetDefault());
            JsonLdContextManager.DefaultUrl = contextUrl;

            // process the request JSON
            json = await JsonLdProcessor.Compact(json, contextUrl);

            // convert the JSON to a resource
            return (Resource)json.ToObject(type);
        }

        /// <summary>
        /// Renders a resource to JSON using the provided JSON LD context
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="contextUrl"></param>
        /// <returns></returns>
        public async Task<JToken> GetJsonFromResource(Resource resource, string contextUrl)
        {
            if (resource == null)
                return null;

            // create JSON obj
            var jObj = JObject.FromObject(resource);

            // check for specified context in request
            if (contextUrl == null)
                return jObj;

            // process JSON with context and return
            return await JsonLdProcessor.Compact(jObj, contextUrl);
        }
    }
}