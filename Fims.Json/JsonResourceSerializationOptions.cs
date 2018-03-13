using System;
using System.Threading.Tasks;
using Fims.Core.Model;
using Newtonsoft.Json;

namespace Fims.Json
{
    public class JsonResourceSerializationOptions
    {
        /// <summary>
        /// Gets or sets the resource resolver
        /// </summary>
        public Func<string, Task<Resource>> ResourceResolver { get; set; }

        /// <summary>
        /// Gets or sets the JSON serialization settings
        /// </summary>
        public JsonSerializerSettings JsonSerializerSettings { get; set; } = new JsonSerializerSettings();
    }
}