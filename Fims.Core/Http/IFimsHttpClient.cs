using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Fims.Core.Http
{
    public interface IFimsHttpClient
    {
        /// <summary>
        /// Gets a FIMS resource
        /// </summary>
        /// <param name="url"></param>
        /// <param name="contextUrl"></param>
        /// <returns></returns>
        Task<JToken> GetAsync(string url, string contextUrl);

        /// <summary>
        /// Posts a FIMS resource
        /// </summary>
        /// <param name="url"></param>
        /// <param name="obj"></param>
        /// <param name="contextUrl"></param>
        /// <returns></returns>
        Task<JToken> PostAsync(string url, object obj, string contextUrl);

        /// <summary>
        /// Puts a FIMS resource
        /// </summary>
        /// <param name="url"></param>
        /// <param name="obj"></param>
        /// <param name="contextUrl"></param>
        /// <returns></returns>
        Task<JToken> PutAsync(string url, object obj, string contextUrl);

        /// <summary>
        /// Deletes a FIMS resource
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="contextUrl"></param>
        /// <returns></returns>
        Task<JToken> DeleteAsync(string url, string contextUrl);
    }
}
