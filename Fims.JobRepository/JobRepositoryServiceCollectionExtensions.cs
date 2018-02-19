using Fims.Core.Model;
using Fims.Server.Business;
using Microsoft.Extensions.DependencyInjection;

namespace Fims.JobRepository
{
    public static class JobRepositoryServiceCollectionExtensions
    {
        /// <summary>
        /// Adds resource handling for a FIMS job repository
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddFimsJobRepository(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddFimsResourceHandling(
                options =>
                    options.Register<AmeJob>()
                           .Register<CaptureJob>()
                           .Register<QaJob>()
                           .Register<TransferJob>()
                           .Register<TransformJob>());
        }
    }
}
