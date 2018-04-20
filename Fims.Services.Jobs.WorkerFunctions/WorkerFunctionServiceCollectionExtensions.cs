using Fims.Server.Business;
using Microsoft.Extensions.DependencyInjection;

namespace Fims.Services.Jobs.WorkerFunctions
{
    public static class WorkerFunctionServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the FIMS worker function API
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddFimsWorkerFunctionApi<T>(this IServiceCollection serviceCollection)
            where T : class, IWorkerFunctionInvoker
        {
            return serviceCollection.AddFimsResourceHandling<WorkerFunctionInvocation>()
                                    .AddScoped<IWorkerFunctionInvoker, T>();
        }
        
        /// <summary>
        /// Adds the FIMS worker function API with in-process worker processing
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddFimsInProcesssWorkerFunctionApi<T>(this IServiceCollection serviceCollection) where T : class, IWorker
        {
            return serviceCollection.AddFimsResourceHandling<WorkerFunctionInvocation>()
                                    .AddScoped<IWorkerFunctionInvoker>(svcProvider => new InProcessWorkerFunctionInvoker(svcProvider.GetService))
                                    .AddScoped<T>();
        }
    }
}