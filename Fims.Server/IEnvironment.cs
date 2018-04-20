namespace Fims.Server
{
    public interface IEnvironment
    {
        /// <summary>
        /// Allows mapping of alternate keys to get values from config
        /// </summary>
        /// <param name="key"></param>
        /// <param name="alternateKey"></param>
        void AddAlternateKey(string key, string alternateKey);

        /// <summary>
        /// Gets the value of an environment variable with the given key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);
    }
}