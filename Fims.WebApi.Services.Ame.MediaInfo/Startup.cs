using Fims.Server;
using Fims.Server.Api;
using Fims.Server.Files;
using Fims.Server.LiteDb;
using Fims.Services.Ame.MediaInfo;
using Fims.Services.Jobs.WorkerFunctions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fims.WebApi.Services.Ame.MediaInfo
{
    public class Startup
    {
        /// <summary>
        /// Instantiates <see cref="Startup"/>
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            // set worker function name
            configuration[nameof(WorkerFunctionEnvironmentExtensions.WorkerFunctionName)] = typeof(MediaInfoWorker).AssemblyQualifiedName;
        }

        /// <summary>
        /// Configures FIMS services
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddConsoleLogger()
                    .AddLocalFileStorage()
                    .AddLiteDb()
                    .AddLocalMediaInfo()
                    .AddFimsInProcessWorkerFunctionWebApi<MediaInfoWorker>();
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
