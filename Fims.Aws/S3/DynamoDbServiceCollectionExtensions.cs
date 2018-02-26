using Fims.Server.Data;
using Fims.Services.Files;
using Microsoft.Extensions.DependencyInjection;

namespace Fims.Aws.S3
{
    public static class S3ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds DynamoDB as the repository service behind FIMS
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddS3FileStorage(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddScoped<IFileStorage, S3FileStorage>();
        }
    }
}