using System;
using System.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.DependencyInjection;

namespace Fims.Azure.DependencyInjection
{
    public abstract class DependencyInjectionExtension : IExtensionConfigProvider
    {
        /// <summary>
        /// Initializes the app by setting up dependency injection
        /// </summary>
        /// <param name="context"></param>
        public void Initialize(ExtensionConfigContext context)
        {
            // allow derived classes to register services
            var services = RegisterServices();
            
            // create injection scope manager
            var injectionScopeManager = new InjectionScopeManager(services.BuildServiceProvider(true));
            
            // create filter for managing scopes
            var injectionScopeFilter = new InjectionScopeFilter(injectionScopeManager);
            context.Config.RegisterExtension<IFunctionInvocationFilter>(injectionScopeFilter);
            context.Config.RegisterExtension<IFunctionExceptionFilter>(injectionScopeFilter);

            // map injection binding provider to the inject attribute
            context.AddBindingRule<InjectAttribute>().Bind(new InjectBindingProvider(injectionScopeManager));
        }

        /// <summary>
        /// Registers services by getting all loaded types that implement <see cref="IStartup"/>
        /// </summary>
        /// <returns></returns>
        private IServiceCollection RegisterServices()
            =>
                AppDomain.CurrentDomain.GetAssemblies()
                         .SelectMany(a => a.GetExportedTypes())
                         .Where(t => typeof(IStartup).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface && t.GetConstructor(Type.EmptyTypes) != null)
                         .Select(t => (IStartup)Activator.CreateInstance(t))
                         .Aggregate<IStartup, IServiceCollection>(new ServiceCollection(), (services, startup) => startup.Configure(services));
    }
}