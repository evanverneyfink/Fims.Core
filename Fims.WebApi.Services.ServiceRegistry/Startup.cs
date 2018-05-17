using Fims.Aws.DynamoDb;
using Fims.Server;
using Fims.Server.Api;
using Fims.Server.Files;
using Fims.Server.LiteDb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fims.WebApi.Services.ServiceRegistry
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configures FIMS services
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddConsoleLogger()
                    .AddLocalFileStorage()
                    .AddDynamoDbFimsRepository(opts => Configuration.Bind("AWS", opts))
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
                app.UseDeveloperExceptionPage().UseIisExpressUrl();

            app.UseFimsWebApi();
        }
    }
}
