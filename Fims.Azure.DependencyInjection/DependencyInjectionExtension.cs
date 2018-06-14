using Microsoft.Azure.WebJobs.Host.Config;

namespace Fims.Azure.DependencyInjection
{
    public class DependencyInjectionExtension : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            context
                .AddBindingRule<InjectAttribute>()
                .Bind(new InjectBindingProvider());
        }
    }
}