using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Fims.Core;
using Fims.Server.Api;

namespace Fims.Azure.Http
{
    public class HttpRequestMessageRequest : IRequest
    {
        /// <summary>
        /// Instantiates a <see cref="HttpRequestMessageRequest"/>
        /// </summary>
        /// <param name="requestMessage"></param>
        public HttpRequestMessageRequest(HttpRequestMessage requestMessage)
        {
            RequestMessage = requestMessage;
        }

        /// <summary>
        /// Gets the underlying request
        /// </summary>
        private System.Net.Http.HttpRequestMessage RequestMessage { get; }

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
        public IResponse Response { get; } = new HttpResponseMessageResponse();

        /// <summary>
        /// Reads the body of the reqeust as JSON
        /// </summary>
        /// <returns></returns>
        public Task<string> ReadBodyAsText() => RequestMessage.Content.ReadAsStringAsync();
    }
}