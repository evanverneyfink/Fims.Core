using Fims.Server;
using Fims.Server.Api;
using Fims.Server.Files;
using Fims.Server.LiteDb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fims.WebApi.Services.Jobs.JobRepository
{
    public class Startup
    {
        /// <summary>
        /// Instantiates the <see cref="Startup"/> object
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration
        /// </summary>
        private IConfiguration Configuration { get; }

        /// <summary>
        /// Configures FIMS services
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddConsoleLogger()
                    .AddLocalFileStorage()
                    .AddLiteDb()
                    .AddFimsWebApi<Fims.Services.Jobs.JobRepository.JobRepository>(Configuration);
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
