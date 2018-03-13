using Microsoft.Extensions.DependencyInjection;

namespace Fims.Server
{
    public static class ConsoleLoggerServiceCollectionExtensions
    {
        public static IServiceCollection AddConsoleLogger(this IServiceCollection services)
        {
            return services.AddSingleton<ILogger, ConsoleLogger>();
        }
    }
}