using System;
using System.Threading.Tasks;
using Fims.Core.Model;
using Newtonsoft.Json.Linq;

namespace Fims.Core.JsonLd
{
    public interface IJsonLdResourceHelper
    {
        /// <summary>
        /// Gets a resource from the JSON in the body of the request
        /// </summary>
        /// <param name="json"></param>
        /// <param name="type"></param>
        /// <param name="contextBaseUrl"></param>
        /// <returns></returns>
        Task<Resource> GetResourceFromJson(JToken json, Type type, string contextBaseUrl);

        /// <summary>
        /// Renders a resource to JSON using the provided JSON LD context
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="contextUrl"></param>
        /// <returns></returns>
        Task<JToken> GetJsonFromResource(Resource resource, string contextUrl);
    }
}
