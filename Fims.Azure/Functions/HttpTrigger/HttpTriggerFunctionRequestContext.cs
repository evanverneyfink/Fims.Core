using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Fims.Core;
using Fims.Server.Api;

namespace Fims.Azure.Functions.HttpTrigger
{
    public class HttpTriggerFunctionRequestContext : IRequestContext
    {
        /// <summary>
        /// Instantiates a <see cref="HttpTriggerFunctionRequestContext"/>
        /// </summary>
        /// <param name="requestMessage"></param>
        public HttpTriggerFunctionRequestContext(HttpRequestMessage requestMessage)
        {
            RequestMessage = requestMessage;
        }

        /// <summary>
        /// Gets the underlying request
        /// </summary>
        private HttpRequestMessage RequestMessage { get; }

        /// <summary>
        /// Gets the HTTP method
        /// </summary>
        public string Method => RequestMessage.Method.ToString();

        /// <summary>
        /// Gets the path of the request
        /// </summary>
        public string Path => RequestMessage.RequestUri.ToString();

        /// <summary>
        /// Gets the dictionary of query string parameters
        /// </summary>
        public IDictionary<string, string> QueryParameters => RequestMessage.RequestUri.QueryParameters();

        /// <summary>
        /// Gets the response to be sent back to the requester
        /// </summary>
        /// <returns></returns>
        public IResponse Response { get; } = new HttpTriggerFunctionResponse();

        /// <summary>
        /// Reads the body of the reqeust as JSON
        /// </summary>
        /// <returns></returns>
        public Task<string> ReadBodyAsText() => RequestMessage.Content.ReadAsStringAsync();
    }
}