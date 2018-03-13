using Microsoft.Extensions.DependencyInjection;

namespace Fims.Server.Api
{
    public static class FimsServerApiServiceCollectionExtensions
    {
        public static IServiceCollection AddFimsServerDefaultApi(this IServiceCollection serviceCollection)
        {
            return
                serviceCollection.AddScoped<IUrlSegmentResourceMapper, DefaultUrlSegmentResourceMapper>()
                                 .AddScoped<IResourceDescriptorHelper, DefaultResourceDescriptorHelper>()
                                 .AddScoped<IRequestHandler, DefaultRequestHandler>();
        }
    }
}
