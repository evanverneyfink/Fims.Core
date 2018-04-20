﻿using Fims.Server;
using Microsoft.Extensions.Configuration;

namespace Fims.WebApi
{
    public class WebApiEnvironment : Environment
    {
        /// <summary>
        /// Instantiates a <see cref="WebApiEnvironment"/>
        /// </summary>
        /// <param name="configuration"></param>
        public WebApiEnvironment(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        /// <summary>
        /// Gets the configuration
        /// </summary>
        private IConfiguration Configuration { get; }

        /// <summary>
        /// Gets an environment setting from configuration
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override string GetTextValue(string key)
        {
            return Configuration[key];
        }
    }
}