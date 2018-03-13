using System;
using Fims.Core;
using Fims.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Configuration;

namespace Fims.WebApi
{
    public class WebApiEnvironment : IEnvironment
    {
        /// <summary>
        /// Instantiates a <see cref="WebApiEnvironment"/>
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        /// <param name="configuration"></param>
        public WebApiEnvironment(HostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            HostingEnvironment = hostingEnvironment;
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the hosting environment
        /// </summary>
        private HostingEnvironment HostingEnvironment { get; }

        /// <summary>
        /// Gets the configuration
        /// </summary>
        private IConfiguration Configuration { get; }

        /// <summary>
        /// Gets an environment setting from configuration
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            // use localhost + port for dev public url
            if (key == nameof(EnvironmentExtensions.PublicUrl) && HostingEnvironment.IsDevelopment())
                return $"http://localhost:{Environment.GetEnvironmentVariable("ASPNETCORE_PORT")}".Parse<T>();

            return Configuration[key].TryParse<T>(out var val) ? val : default(T);
        }
    }
}