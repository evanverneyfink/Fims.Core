using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;
using Fims.Core.Model;
using Fims.Core.Serialization;
using Newtonsoft.Json.Linq;

namespace Fims.Aws.DynamoDb
{
    public static class DynamoDbExtensions
    {
        /// <summary>
        /// Converts a <see cref="Document"/> to an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        public static async Task<T> ToObject<T>(this IResourceSerializer serializer, Document document) where T : Resource
        {
            return (T)await serializer.ToObject(document, typeof(T));
        }

        /// <summary>
        /// Converts a <see cref="Document"/> to an object
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="document"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static async Task<Resource> ToObject(this IResourceSerializer serializer, Document document, Type type)
        {
            var json = document.ToJson();
            Console.WriteLine("[DynamoDbExtensions.ToObject] Document JSON = {0}", json);

            var jObj = JObject.Parse(json).Unwrap();
            Console.WriteLine("[DynamoDbExtensions.ToObject] Unwrapped JSON = {0}", jObj);

            return await serializer.Deserialize(jObj.ToString(), type);
        }

        /// <summary>
        /// Converts an object to a <see cref="Document"/>
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public static Document ToDocument<T>(this IResourceSerializer serializer, T resource) where T : Resource
        {
            Console.WriteLine("[DynamoDbExtensions.ToDocument] Object JSON = {0}", resource);

            var jObj = serializer.Wrap(resource);
            Console.WriteLine("[DynamoDbExtensions.ToDocument] Wrapped JSON = {0}", jObj);

            return Document.FromJson(jObj.ToString());
        }

        /// <summary>
        /// Wraps a resource into a DynamoDB document
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public static JObject Wrap(this IResourceSerializer serializer, Resource resource)
        {
            return new JObject
            {
                [DynamoDbDefaults.ResourceTypeAttribute] = resource.GetType().Name,
                [DynamoDbDefaults.ResourceIdAttribute] = resource.Id,
                [DynamoDbDefaults.ResourceAttribute] = JObject.Parse(serializer.Serialize(resource))
            };
        }

        /// <summary>
        /// Unwraps a resource from a DynamoDB document
        /// </summary>
        /// <param name="jObj"></param>
        /// <returns></returns>
        public static JObject Unwrap(this JObject jObj)
        {
            return jObj[DynamoDbDefaults.ResourceAttribute]?.Value<JObject>();
        }

        /// <summary>
        /// Converts a dictionary of parameters to a <see cref="ScanFilter"/>
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static QueryFilter ToQueryFilter(this IDictionary<string, string> filters)
        {
            var scanFilter = new QueryFilter();

            if (filters != null)
                foreach (var kvp in filters)
                    scanFilter.AddCondition(kvp.Key, ScanOperator.Equal, new Primitive(kvp.Value));

            return scanFilter;
        }
    }
}