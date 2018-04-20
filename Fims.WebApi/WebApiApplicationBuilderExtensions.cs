using Fims.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Fims.WebApi
{
    public static class WebApiApplicationBuilderExtensions
    {
        /// <summary>
        /// The key for IIS express url setting
        /// </summary>
        private const string IisExpressUrlSettingKey = "iisSettings:iisExpress:applicationUrl";

        /// <summary>
        /// Adds the IIS express local url setting as an alternate for the PublicUrl environment setting
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseIisExpressUrl(this IApplicationBuilder app)
        {
            app.ApplicationServices.GetRequiredService<IEnvironment>().AddAlternateKey(nameof(EnvironmentExtensions.PublicUrl), IisExpressUrlSettingKey);
            return app;
        }
    }
}