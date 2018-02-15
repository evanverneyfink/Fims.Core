using Microsoft.Extensions.DependencyInjection;

namespace Fims.Server.Api
{
    public static class FimsServerApiServiceCollectionExtensions
    {
        public static IServiceCollection AddFimsServerDefaultApi(this IServiceCollection serviceCollection)
        {
            return
                serviceCollection.AddSingleton<ILogger, ConsoleLogger>()
                                 .AddScoped<IUrlSegmentResourceMapper, DefaultUrlSegmentResourceMapper>()
                                 .AddScoped<IResourceUrlParser, DefaultResourceUrlParser>()
                                 .AddScoped<IRequestHandler, DefaultRequestHandler>();
        }
    }
}
