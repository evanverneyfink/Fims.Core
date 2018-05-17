using System.Net;
using System.Net.Http;
using System.Text;
using Fims.Server;
using Fims.Server.Api;
using Newtonsoft.Json.Linq;

namespace Fims.Azure.Functions.HttpTrigger
{
    public class HttpTriggerFunctionResponse : IHttpTriggerFunctionResponse
    {
        /// <summary>
        /// Gets the underlying response
        /// </summary>
        private HttpResponseMessage Response { get; } = new HttpResponseMessage();

        /// <summary>
        /// Sets the status of the response
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public IResponse WithStatus(HttpStatusCode status)
        {
            Response.StatusCode = status;
            return this;
        }

        /// <summary>
        /// Sets a header on the response
        /// </summary>
        /// <param name="header"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IResponse WithHeader(string header, string value)
        {
            if (Response.Headers.Contains(header))
                Response.Headers.Remove(header);

            Response.Headers.Add(header, value);

            return this;
        }

        /// <summary>
        /// Sets the body on the response to plain text
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public IResponse WithPlainTextBody(string message)
        {
            Response.Content = new StringContent(message, Encoding.UTF8, "text/plain");
            return this;
        }

        /// <summary>
        /// Sets the body on the response to JSON
        /// </summary>
        /// <param name="jToken"></param>
        /// <returns></returns>
        public IResponse WithJsonBody(JToken jToken)
        {
            Response.Content = new JsonContent(jToken);
            return this;
        }

        /// <summary>
        /// Gets the response as an <see cref="HttpResponseMessage"/>
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage AsHttpResponseMessage() => Response;
    }
}