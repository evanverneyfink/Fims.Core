using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Fims.Server.Api
{
    public interface IRequestContext
    {
        /// <summary>
        /// Gets the HTTP method
        /// </summary>
        string Method { get; }

        /// <summary>
        /// Gets the path of the request
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Gets the dictionary of query string parameters
        /// </summary>
        IDictionary<string, string> QueryParameters { get; }

        /// <summary>
        /// Reads the body of the reqeust as JSON
        /// </summary>
        /// <returns></returns>
        Task<string> ReadBodyAsText();
        
        /// <summary>
        /// Creates a response to be sent back to the requester
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        IResponse CreateResponse(HttpStatusCode status = HttpStatusCode.OK);
    }
}