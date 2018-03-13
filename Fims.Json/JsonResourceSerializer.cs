using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fims.Core;
using Fims.Core.Model;
using Fims.Core.Serialization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Fims.Json
{
    internal class JsonResourceSerializer : IResourceSerializer
    {
        /// <summary>
        /// Instantiates a <see cref="JsonResourceSerializer"/>
        /// </summary>
        /// <param name="options"></param>
        public JsonResourceSerializer(IOptions<JsonResourceSerializationOptions> options)
        {
            Options = options?.Value ?? new JsonResourceSerializationOptions();
        }

        /// <summary>
        /// Gets the JSON serialization settings
        /// </summary>
        private JsonResourceSerializationOptions Options { get; }

        /// <summary>
        /// Serializes a resource to JSON
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="linksOnly"></param>
        /// <returns></returns>
        public string Serialize(Resource resource, bool linksOnly = true)
        {
            var json = new StringBuilder();

            var serializer = JsonSerializer.CreateDefault(Options.JsonSerializerSettings);

            using (var writer = new JsonTextWriter(new StringWriter(json)))
            {
                WriteResource(writer, serializer, resource, linksOnly);

                writer.Flush();
            }

            return json.ToString();
        }

        /// <summary>
        /// Writes a <see cref="Resource"/> as JSON
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="serializer"></param>
        /// <param name="resource"></param>
        /// <param name="linksOnly"></param>
        private void WriteResource(JsonWriter writer, JsonSerializer serializer, Resource resource, bool linksOnly)
        {
            // write the start of the JSON object
            writer.WriteStartObject();

            foreach (var prop in resource.GetType().GetProperties())
            {
                // write the property
                writer.WritePropertyName(prop.Name.PascalCaseToCamelCase());

                var propValue = prop.GetValue(resource);

                if (propValue == null)
                    writer.WriteNull();
                else if (typeof(Resource).IsAssignableFrom(prop.PropertyType))
                {
                    // get the child resource
                    var child = (Resource)propValue;

                    // write either the link/ID or the entire object, based on the provided flag
                    if (linksOnly)
                        serializer.Serialize(writer, child.Id);
                    else
                        WriteResource(writer, serializer, child, false);
                }
                else if (prop.PropertyType.IsAssignableToEnumerableOf<Resource>())
                {
                    //  get child collection
                    var children = ((IEnumerable)prop.GetValue(resource))?.OfType<Resource>();
                    if (children == null)
                        continue;

                    // write start of JSON array
                    writer.WriteStartArray();

                    // foreach object, we're either going to write the link/ID or the whole object
                    foreach (var child in children)
                    {
                        if (linksOnly)
                            serializer.Serialize(writer, child.Id);
                        else
                            WriteResource(writer, serializer, child, false);
                    }

                    // write end of JSON array
                    writer.WriteEndArray();
                }
                else if (prop.PropertyType.IsAssignableToEnumerableOf<Type>())
                    serializer.Serialize(writer, ((IEnumerable<Type>)propValue).Select(t => t.ToResourceTypeName()).ToArray());
                else if (prop.PropertyType == typeof(Type))
                    serializer.Serialize(writer, ((Type)propValue).ToResourceTypeName());
                else
                    serializer.Serialize(writer, propValue);
            }

            // write the end of the JSON object
            writer.WriteEndObject();
        }

        /// <summary>
        /// Deserializes a resource from JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serialized"></param>
        /// <param name="resolveLinks"></param>
        /// <returns></returns>
        public async Task<T> Deserialize<T>(string serialized, bool resolveLinks = true) where T : Resource, new()
        {
            return (T)await Deserialize(serialized, typeof(T), resolveLinks);
        }

        /// <summary>
        /// Deserializes a resource from text
        /// </summary>
        /// <param name="serialized"></param>
        /// <param name="type"></param>
        /// <param name="resolveLinks"></param>
        /// <returns></returns>
        public async Task<Resource> Deserialize(string serialized, Type type, bool resolveLinks = true)
        {
            // parse a JSON object first
            var jObj = JObject.Parse(serialized);

            // convert the object
            return await ReadResource(jObj, type, resolveLinks);
        }

        /// <summary>
        /// Reads a <see cref="Resource"/> from JSON
        /// </summary>
        /// <param name="jObj"></param>
        /// <param name="type"></param>
        /// <param name="resolveLinks"></param>
        /// <returns></returns>
        public async Task<Resource> ReadResource(JObject jObj, Type type, bool resolveLinks)
        {
            // ensure the type is a resource type
            if (!typeof(Resource).IsAssignableFrom(type))
                throw new Exception(
                    $"Type {type.Name} cannot be deserialized from JSON because it does not inherit from type {typeof(Resource).Name}");

            // get the empty constructor (if any)
            var ctor = type.GetConstructor(Type.EmptyTypes);
            if (ctor == null)
                throw new Exception($"Type {type.Name} cannot be deserialized from JSON because it does not have a parameterless constructor.");

            // create new instance of resource types
            var resource = (Resource)ctor.Invoke(null);
            var resourceProperties = type.GetProperties();

            foreach (var jsonProp in jObj.Properties())
            {
                // find the matching property on the object
                var prop = resourceProperties.FirstOrDefault(p => p.Name.Equals(jsonProp.Name, StringComparison.OrdinalIgnoreCase));
                if (prop == null)
                    continue;

                if (typeof(Resource).IsAssignableFrom(prop.PropertyType))
                {
                    // get child object
                    var childObj = jsonProp.Value.Type == JTokenType.String
                                       ? new JObject {[nameof(Resource.Id)] = jsonProp.Value.Value<string>()}
                                       : (JObject)jsonProp.Value;
                    
                    prop.SetValue(resource, await ReadResource(childObj, prop.PropertyType, resolveLinks));
                }
                else if (prop.PropertyType.IsAssignableToEnumerableOf<Resource>())
                {
                    var childType = prop.PropertyType.GenericTypeArguments[0];

                    // create list of child objects
                    var childCollection =
                        (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(childType));

                    // get child object
                    var jArray = jsonProp.Value.Type == JTokenType.Array
                                     ? new JArray(((JArray)jsonProp.Value).Select(GetOrWrap))
                                     : new JArray {GetOrWrap(jsonProp.Value)};

                    // create resource from each object in the array
                    foreach (var childJObj in jArray.OfType<JObject>())
                        childCollection.Add(await ReadResource(childJObj, childType, resolveLinks));
                        
                    // set the child collection
                    prop.SetValue(resource, childCollection);
                }
                else if (prop.PropertyType.IsGenericEnumerable())
                {
                    var childType = prop.PropertyType.GenericTypeArguments[0];

                    // get child object
                    var jArray = jsonProp.Value.Type == JTokenType.Array ? (JArray)jsonProp.Value : new JArray {jsonProp.Value};

                    // create child list
                    var childCollection =
                        (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(childType));

                    // deserialize and add children to collection
                    foreach (var childItem in jArray)
                        childCollection.Add(childType == typeof(Type)
                                                ? childItem.Value<string>().ToResourceType()
                                                : childItem.Value<string>().Parse(childType));
                    
                    // set the child collection on the resource
                    prop.SetValue(resource, childCollection);
                }
                else if (prop.PropertyType == typeof(Type))
                {
                    prop.SetValue(resource, jsonProp.Value.Value<string>().ToResourceType());
                }
                else
                {
                    prop.SetValue(resource, jsonProp.Value.Value<string>().Parse(prop.PropertyType));
                }
            }

            return resource;
        }

        private JObject GetOrWrap(JToken jToken)
        {
            return jToken.Type == JTokenType.String
                ? new JObject {[nameof(Resource.Id)] = jToken.Value<string>()}
                : (JObject)jToken;
        }
    }
}
