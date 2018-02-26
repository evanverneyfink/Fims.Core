using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DocumentModel;
using Fims.Core.Model;
using Newtonsoft.Json.Linq;

namespace Fims.Aws.DynamoDb
{
    public static class DynamoDbExtensions
    {
        /// <summary>
        /// Converts a <see cref="Document"/> to an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="document"></param>
        /// <returns></returns>
        public static T ToObject<T>(this Document document) where T : Resource
        {
            var json = document.ToJson();
            Console.WriteLine("[DynamoDbExtensions.ToObject] Document JSON = {0}", json);

            var jObj = JObject.Parse(json).Unwrap();
            Console.WriteLine("[DynamoDbExtensions.ToObject] Unwrapped JSON = {0}", jObj);

            return (T)Resource.FromToken(jObj, typeof(T));
        }

        /// <summary>
        /// Converts an object to a <see cref="Document"/>
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public static Document ToDocument<T>(this T resource) where T : Resource
        {
            Console.WriteLine("[DynamoDbExtensions.ToDocument] Object JSON = {0}", resource);

            var jObj = resource.Wrap();
            Console.WriteLine("[DynamoDbExtensions.ToDocument] Wrapped JSON = {0}", jObj);

            return Document.FromJson(jObj.ToString());
        }

        /// <summary>
        /// Wraps a resource into a DynamoDB document
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public static JObject Wrap(this Resource resource)
        {
            return new JObject
            {
                [DynamoDbDefaults.ResourceTypeAttribute] = resource.Type,
                [DynamoDbDefaults.ResourceIdAttribute] = resource.Id,
                [DynamoDbDefaults.ResourceAttribute] = resource
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