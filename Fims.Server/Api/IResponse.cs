using Newtonsoft.Json.Linq;

namespace Fims.Server.Api
{
    public interface IResponse
    {
        /// <summary>
        /// Sets a header on the response
        /// </summary>
        /// <param name="header"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IResponse WithHeader(string header, string value);

        /// <summary>
        /// Sets the body on the response to plain text
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        IResponse WithPlainTextBody(string message);

        /// <summary>
        /// Sets the body on the response to JSON
        /// </summary>
        /// <param name="jToken"></param>
        /// <returns></returns>
        IResponse WithJsonBody(JToken jToken);
    }


}