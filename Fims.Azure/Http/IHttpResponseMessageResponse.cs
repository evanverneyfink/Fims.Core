using System.Net.Http;
using Fims.Server.Api;

namespace Fims.Azure.Http
{
    public interface IHttpResponseMessageResponse : IResponse
    {
        /// <summary>
        /// Gets the response as an <see cref="HttpResponseMessage"/>
        /// </summary>
        /// <returns></returns>
        HttpResponseMessage AsHttpResponseMessage();
    }
}