using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fims.Core.Http;
using Fims.Core.JsonLd;
using Fims.Core.Model;
using Microsoft.Extensions.Options;

namespace Fims.Core.Services
{
    public class ServiceManager : IServiceManager
    {
        /// <summary>
        /// Instantiates a <see cref="ServiceManager"/>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="options"></param>
        public ServiceManager(IFimsHttpClient httpClient, IOptions<ServiceRegistryOptions> options)
        {
            HttpClient = httpClient;
            Options = options.Value ?? new ServiceRegistryOptions();
        }

        private ServiceRegistryOptions Options { get; }

        /// <summary>
        /// Gets the FIMS HTTP client
        /// </summary>
        private IFimsHttpClient HttpClient { get;  }

        /// <summary>
        /// Gets all services
        /// </summary>
        /// <param name="contextUrl"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Service>> GetServicesAsync(string contextUrl = null)
        {
            // ensure service registry url is set
            if (string.IsNullOrWhiteSpace(Options.ServiceRegistryUrl))
                throw new Exception("Service Registry Services URL not set");

            // do a get against the service registry url
            var services = await HttpClient.GetAsync(Options.ServiceRegistryUrl, contextUrl ?? Contexts.Default.Url);

            // convert results to service objects
            return services.Select(jObj => jObj.ToObject<Service>());
        }

        /// <summary>
        /// Gets all urls for a given resource type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetResourceTypeUrlsAsync(string type)
        {
            // get all services
            var services = await GetServicesAsync();

            // get resources for each service that match the given type, then pull their http endpoints
            return services.SelectMany(s => s.Resources.Where(r => r.ResourceType.Equals(type, StringComparison.OrdinalIgnoreCase))).Select(r => r.HttpEndpoint);
        }
    }
}