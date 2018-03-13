using System;
using System.Threading.Tasks;
using Fims.Core.Model;

namespace Fims.Core.Serialization
{
    public interface IResourceSerializer
    {
        /// <summary>
        /// Serializes a resource to text
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="linksOnly"></param>
        /// <returns></returns>
        string Serialize(Resource resource, bool linksOnly = true);

        /// <summary>
        /// Deserializes a resource from text
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serialized"></param>
        /// <param name="resolveLinks"></param>
        /// <returns></returns>
        Task<T> Deserialize<T>(string serialized, bool resolveLinks = false) where T : Resource, new();

        /// <summary>
        /// Deserializes a resource from text
        /// </summary>
        /// <param name="serialized"></param>
        /// <param name="type"></param>
        /// <param name="resolveLinks"></param>
        /// <returns></returns>
        Task<Resource> Deserialize(string serialized, Type type, bool resolveLinks = false);
    }
}