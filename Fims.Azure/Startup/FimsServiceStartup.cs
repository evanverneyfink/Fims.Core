using Fims.Azure.DependencyInjection;
using Fims.Server;
using Fims.Server.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Fims.Azure.Startup
{
    public abstract class FimsServiceStartup : IStartup
    {
        /// <summary>
        /// Configures services for the application
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public IServiceCollection Configure(IServiceCollection services)
        {
            services
                .AddSingleton<ILogger, ConsoleLogger>()
                .AddFimsResourceDataHandling();

            return RegisterAdditionalServices(services);
        }

        /// <summary>
        /// Registers any additional services the application might need
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        protected abstract IServiceCollection RegisterAdditionalServices(IServiceCollection services);
    }
}