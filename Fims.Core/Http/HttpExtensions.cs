using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Fims.Core.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Fims.Core.Http
{
    public static class HttpExtensions
    {
        /// <summary>
        /// Reads an HTTP response's content as FIMS JSON
        /// </summary>
        /// <param name="httpContent"></param>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="compactAsync"></param>
        /// <returns></returns>
        public static async Task<JToken> ReadAsJsonAsync(this HttpContent httpContent, HttpMethod method, string url, Func<JToken, Task<JToken>> compactAsync)
        {
            var jsonText = await httpContent.ReadAsStringAsync();

            if (JsonConvert.DeserializeObject(jsonText) is JToken jToken)
            {
                if (jToken is JArray jArray)
                    return new JArray(await Task.WhenAll(jArray.Select(compactAsync)));

                if (jToken is JObject jObj)
                    return await compactAsync(jObj);
            }

            throw new Exception($"Unrecognized JSON object returned by {method} {url}.");
        }

        /// <summary>
        /// Reads an HTTP response's content as FIMS resource
        /// </summary>
        /// <param name="httpContent"></param>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="compactAsync"></param>
        /// <returns></returns>
        public static async Task<T> ReadAsResourceAsync<T>(this HttpContent httpContent,
                                                           HttpMethod method,
                                                           string url,
                                                           Func<JToken, Task<T>> compactAsync)
            where T : Resource
        {
            var jsonText = await httpContent.ReadAsStringAsync();

            if (JsonConvert.DeserializeObject(jsonText) is JToken jToken)
            {
                if (jToken is JArray)
                    throw new Exception($"Expected single JSON object but found JSON array returned by {method} {url}");

                if (jToken is JObject jObj)
                    return await compactAsync(jObj);
            }

            throw new Exception($"Unrecognized JSON object returned by {method} {url}.");
        }

        /// <summary>
        /// Reads an HTTP response's content as FIMS resource
        /// </summary>
        /// <param name="httpContent"></param>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="compactAsync"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<T>> ReadAsResourceCollectionAsync<T>(this HttpContent httpContent,
                                                                                  HttpMethod method,
                                                                                  string url,
                                                                                  Func<JToken, Task<T>> compactAsync)
            where T : Resource
        {
            var jsonText = await httpContent.ReadAsStringAsync();

            if (JsonConvert.DeserializeObject(jsonText) is JToken jToken)
            {
                if (jToken is JArray jArray)
                    return await Task.WhenAll(jArray.Select(compactAsync));

                if (jToken is JObject)
                    throw new Exception($"Expected JSON array but found single JSON object returned by {method} {url}");
            }

            throw new Exception($"Unrecognized JSON object returned by {method} {url}.");
        }
    }
}