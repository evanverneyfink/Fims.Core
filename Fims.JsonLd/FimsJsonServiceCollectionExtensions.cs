using JsonLD.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Fims.JsonLd
{
    public static class FimsJsonServiceCollectionExtensions
    {
        public static IServiceCollection AddFimsCore(this IServiceCollection serviceCollection)
        {
            return
                serviceCollection.AddOptions()
                                 .AddSingleton<IDocumentLoader, CachedDocumentLoader>()
                                 .AddSingleton<IJsonLdContextManager, JsonLdContextManager>()
                                 .Configure<JsonLdContextManagerOptions>(opts => { opts.DefaultContextUrl = Contexts.Default.Url; })
                                 .AddSingleton<IJsonLdProcessor, JsonLdProcessor>()
                                 .AddSingleton<IJsonLdResourceHelper, JsonLdResourceHelper>();
        }
    }
}
