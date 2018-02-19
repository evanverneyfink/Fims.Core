using Microsoft.Extensions.DependencyInjection;

namespace Fims.Server.Data
{
    public static class ResourceDataHandlingServiceCollectionExtensions
    {
        /// <summary>
        /// Adds resource data handling
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddFimsResourceDataHandling(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                   .AddScoped(typeof(IRepositoryResourceDataHandler), typeof(RepositoryResourceDataHandler))
                   .AddScoped(typeof(IHttpResourceDataHandler), typeof(HttpResourceDataHandler))
                   .AddScoped(typeof(IResourceDataHandler), typeof(ResourceDataHandler));
        }
    }
}