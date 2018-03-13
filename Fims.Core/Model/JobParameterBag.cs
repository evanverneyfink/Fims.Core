using System.Collections;
using System.Collections.Generic;

namespace Fims.Core.Model
{
    public class JobParameterBag : Resource, IDictionary<string, object>
    {
        /// <summary>
        /// Gets the underlying dictionary
        /// </summary>
        private IDictionary<string, object> UnderlyingDictionary { get; } = new Dictionary<string, object>();

        #region Dictionary Implementation

        public int Count => UnderlyingDictionary.Count;

        public bool IsReadOnly => UnderlyingDictionary.IsReadOnly;

        public ICollection<string> Keys => UnderlyingDictionary.Keys;

        public ICollection<object> Values => UnderlyingDictionary.Values;

        public object this[string key]
        {
            get => UnderlyingDictionary[key];
            set => UnderlyingDictionary[key] = value;
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => UnderlyingDictionary.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item) => UnderlyingDictionary.Add(item);

        bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item) => UnderlyingDictionary.Contains(item);

        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item) => UnderlyingDictionary.Remove(item);

        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) => UnderlyingDictionary.CopyTo(array, arrayIndex);

        public bool ContainsKey(string key) => UnderlyingDictionary.ContainsKey(key);

        public void Add(string key, object value) => UnderlyingDictionary.Add(key, value);

        public bool Remove(string key) => UnderlyingDictionary.Remove(key);

        public bool TryGetValue(string key, out object value) => UnderlyingDictionary.TryGetValue(key, out value);

        public void Clear() => UnderlyingDictionary.Clear();

        #endregion
    }
}