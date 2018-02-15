using System.Collections.Generic;
using System.Threading.Tasks;
using Fims.Core.Model;

namespace Fims.Core.Services
{
    public interface IServiceManager
    {
        /// <summary>
        /// Gets all services
        /// </summary>
        /// <param name="contextUrl"></param>
        /// <returns></returns>
        Task<IEnumerable<Service>> GetServicesAsync(string contextUrl = null);

        /// <summary>
        /// Gets all urls for a given resource type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> GetResourceTypeUrlsAsync(string type);
    }
}
