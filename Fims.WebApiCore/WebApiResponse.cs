using System.IO;
using Fims.Server.Api;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace Fims.WebApi
{
    public class WebApiResponse : IResponse
    {
        /// <summary>
        /// Instantiates a <see cref="WebApiResponse"/>
        /// </summary>
        /// <param name="response"></param>
        public WebApiResponse(HttpResponse response)
        {
            Response = response;
        }

        /// <summary>
        /// Gets the underlying response
        /// </summary>
        private HttpResponse Response { get; }

        /// <summary>
        /// Sets a header on the response
        /// </summary>
        /// <param name="header"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IResponse WithHeader(string header, string value)
        {
            Response.Headers[header] = value;
            return this;
        }

        /// <summary>
        /// Sets the body on the response to plain text
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public IResponse WithPlainTextBody(string message)
        {
            using (var streamWriter = new StreamWriter(Response.Body))
                streamWriter.Write(message);
            return this;
        }

        /// <summary>
        /// Sets the body on the response to JSON
        /// </summary>
        /// <param name="jToken"></param>
        /// <returns></returns>
        public IResponse WithJsonBody(JToken jToken)
        {
            using (var streamWriter = new StreamWriter(Response.Body))
                streamWriter.Write(jToken.ToString());
            return this;
        }
    }
}