using Fims.Server.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Fims.Aws.DynamoDb
{
    public static class DynamoDbServiceCollectionExtensions
    {
        /// <summary>
        /// Adds DynamoDB as the repository service behind FIMS
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddDynamoDbFimsRepository(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddSingleton<IDynamoDbTableConfigProvider, DefaultDynamoDbTableConfigProvider>()
                                    .AddScoped(typeof(IRepository), typeof(DynamoDbRepository));
        }
    }
}