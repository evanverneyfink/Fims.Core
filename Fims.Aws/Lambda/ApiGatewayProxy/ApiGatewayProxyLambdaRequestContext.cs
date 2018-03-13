﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Fims.Server.Api;

namespace Fims.Aws.Lambda.ApiGatewayProxy
{
    public class ApiGatewayProxyLambdaRequestContext : IRequestContext
    {
        /// <summary>
        /// Instantiates an <see cref="ApiGatewayProxyLambdaRequestContext"/>
        /// </summary>
        /// <param name="request"></param>
        public ApiGatewayProxyLambdaRequestContext(APIGatewayProxyRequest request)
        {
            Request = request;
        }

        /// <summary>
        /// Gets the underlying <see cref="APIGatewayProxyRequest"/>
        /// </summary>
        private APIGatewayProxyRequest Request { get; }

        /// <summary>
        /// Gets the HTTP method from the underlying <see cref="APIGatewayProxyRequest"/>
        /// </summary>
        public string Method => Request.HttpMethod;

        /// <summary>
        /// Gets the path of the request from the underlying <see cref="APIGatewayProxyRequest"/>
        /// </summary>
        public string Path => Request.Path;

        /// <summary>
        /// Gets the dictionary of query string parameters from the underlying <see cref="APIGatewayProxyRequest"/>
        /// </summary>
        public IDictionary<string, string> QueryParameters => Request.QueryStringParameters;

        /// <summary>
        /// Reads the body of the reqeust as JSON from the underlying <see cref="APIGatewayProxyRequest"/>
        /// </summary>
        /// <returns></returns>
        public Task<string> ReadBodyAsText() => Task.FromResult(Request.Body);

        /// <summary>
        /// Gets the response to be sent back to the requester using a <see cref="ApiGatewayProxyLambdaResponse"/>
        /// </summary>
        /// <returns></returns>
        public IResponse Response { get; } = new ApiGatewayProxyLambdaResponse();
    }
}