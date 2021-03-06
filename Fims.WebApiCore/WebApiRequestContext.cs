﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Fims.Server.Api;
using Microsoft.AspNetCore.Http;

namespace Fims.WebApi
{
    public class WebApiRequestContext : IRequestContext
    {
        /// <summary>
        /// Instantiates a <see cref="WebApiRequestContext"/>
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public WebApiRequestContext(IHttpContextAccessor httpContextAccessor)
        {
            HttpContext = httpContextAccessor.HttpContext;
        }

        /// <summary>
        /// Gets the HTTP context
        /// </summary>
        private HttpContext HttpContext { get; }

        /// <summary>
        /// Gets the HTTP method
        /// </summary>
        public string Method => HttpContext.Request.Method;

        /// <summary>
        /// Gets the path of the request
        /// </summary>
        public string Path => HttpContext.Request.Path;

        /// <summary>
        /// Gets the dictionary of query string parameters
        /// </summary>
        public IDictionary<string, string> QueryParameters => HttpContext.Request.Query.ToDictionary(q => q.Key, q => q.Value.ToString());

        /// <summary>
        /// Reads the body of the reqeust as JSON
        /// </summary>
        /// <returns></returns>
        public async Task<string> ReadBodyAsText()
        {
            using (var streamReader = new StreamReader(HttpContext.Request.Body))
                return await streamReader.ReadToEndAsync();
        }

        /// <summary>
        /// Creates a response to be sent back to the requester
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public IResponse CreateResponse(HttpStatusCode status = HttpStatusCode.OK)
        {
            return new WebApiResponse(HttpContext.Response);
        }
    }
}