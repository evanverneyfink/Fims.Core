using System.Collections.Generic;
using System.Linq;
using Fims.Core;

namespace Fims.Server
{
    public abstract class Environment : IEnvironment
    {
        /// <summary>
        /// Gets the mapping of alternate keys
        /// </summary>
        private IDictionary<string, List<string>> AlternateKeys { get; } = new Dictionary<string, List<string>>();

        /// <summary>
        /// Allows mapping of alternate keys to get values from config
        /// </summary>
        /// <param name="key"></param>
        /// <param name="alternateKey"></param>
        public void AddAlternateKey(string key, string alternateKey)
        {
            (AlternateKeys.ContainsKey(key) ? AlternateKeys[key] : (AlternateKeys[key] = new List<string>())).Add(alternateKey);
        }

        /// <summary>
        /// Gets the value of an environment variable with the given key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            foreach (var k in new[] {key}.Concat(AlternateKeys.ContainsKey(key) ? AlternateKeys[key] : new List<string>()))
            {
                var textVal = GetTextValue(k);
                if (textVal != null && textVal.TryParse<T>(out var value))
                    return value;
            }

            return default(T);
        }

        /// <summary>
        /// Gets the text value for a config key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected abstract string GetTextValue(string key);
    }
}