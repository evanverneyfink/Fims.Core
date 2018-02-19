using System;
using Microsoft.Extensions.DependencyInjection;

namespace Fims.JobProcessor
{
    public static class JobProcessorServiceCollectionExtensions
    {
        public static IServiceCollection AddFimsJobProcessor(this IServiceCollection serviceCollection)
        {
            return serviceCollection;
        }
    }
}
