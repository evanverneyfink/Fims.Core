using System.Collections.Generic;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;

namespace Fims.Aws.DynamoDb
{
    public static class DynamoDbExtensions
    {
        /// <summary>
        /// Converts a <see cref="Document"/> to an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="document"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static T ToObject<T>(this Document document, JsonSerializerSettings settings = null)
        {
            return JsonConvert.DeserializeObject<T>(document.ToJson(), settings ?? JsonConvert.DefaultSettings?.Invoke());
        }

        /// <summary>
        /// Converts an object to a <see cref="Document"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static Document ToDocument<T>(this T obj, JsonSerializerSettings settings = null)
        {
            return Document.FromJson(JsonConvert.SerializeObject(obj, settings ?? JsonConvert.DefaultSettings?.Invoke()));
        }

        /// <summary>
        /// Converts a dictionary of parameters to a <see cref="ScanFilter"/>
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static ScanFilter ToScanFilter(this IDictionary<string, string> filters)
        {
            var scanFilter = new ScanFilter();

            if (filters != null)
                foreach (var kvp in filters)
                    scanFilter.AddCondition(kvp.Key, ScanOperator.Equal, new Primitive(kvp.Value));

            return scanFilter;
        }
    }
}