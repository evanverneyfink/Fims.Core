using Fims.Core.Http;
using Fims.Core.JsonLd;
using Fims.Core.Services;
using JsonLD.Core;
using Microsoft.Extensions.DependencyInjection;
using JsonLdProcessor = Fims.Core.JsonLd.JsonLdProcessor;

namespace Fims.Core
{
    public static class FimsCoreServiceCollectionExtensions
    {
        public static IServiceCollection AddFimsCore(this IServiceCollection serviceCollection)
        {
            return
                serviceCollection.AddOptions()
                                 .AddSingleton<IDocumentLoader, CachedDocumentLoader>()
                                 .AddSingleton<IJsonLdContextManager, JsonLdContextManager>()
                                 .Configure<JsonLdContextManagerOptions>(opts => { opts.DefaultContextUrl = Contexts.Default.Url; })
                                 .AddSingleton<IJsonLdProcessor, JsonLdProcessor>()
                                 .AddSingleton<IJsonLdResourceHelper, JsonLdResourceHelper>()
                                 .AddSingleton<IFimsHttpClient, FimsHttpClient>()
                                 .AddSingleton<IServiceManager, ServiceManager>();
        }
    }
}
