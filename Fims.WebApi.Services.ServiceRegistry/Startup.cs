using Fims.Server;
using Fims.Server.Api;
using Fims.Server.Files;
using Fims.Server.LiteDb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Fims.WebApi.Services.ServiceRegistry
{
    public class Startup
    {
        /// <summary>
        /// Configures FIMS services
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddConsoleLogger()
                    .AddLocalFileStorage()
                    .AddLiteDb()
                    .AddFimsWebApi<Fims.Services.ServiceRegistry.ServiceRegistry>();
        }

        /// <summary>
        /// Configures the app to be a FIMS resource API
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseFimsWebApi();
        }
    }
}
