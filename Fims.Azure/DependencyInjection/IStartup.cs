using Microsoft.Extensions.DependencyInjection;

namespace Fims.Azure.DependencyInjection
{
    public interface IStartup
    {
        /// <summary>
        /// Configures services for the application
        /// </summary>
        /// <returns></returns>
        IServiceCollection Configure(IServiceCollection services);
    }
}