using Fims.Server.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Fims.Server.LiteDb
{
    public static class LiteDbServiceCollectionExtensions
    {
        /// <summary>
        /// Adds LiteDB as the backing repository for the service
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddLiteDb(this IServiceCollection services)
        {
            return services.AddScoped<IRepository, LiteDbRepository>();
        }
    }
}