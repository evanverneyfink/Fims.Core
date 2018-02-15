using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Fims.Server.Api
{
    public class FimsMiddleware
    {
        /// <summary>
        /// Instantiates a <see cref="FimsMiddleware"/>
        /// </summary>
        /// <param name="requestHandler"></param>
        /// <param name="next"></param>
        public FimsMiddleware(IRequestHandler requestHandler, RequestDelegate next)
        {
            RequestHandler = requestHandler;
            Next = next;
        }

        /// <summary>
        /// Gets the API layer
        /// </summary>
        private IRequestHandler RequestHandler { get; }

        /// <summary>
        /// Gets the next step in the pipeline
        /// </summary>
        private RequestDelegate Next { get; }

        /// <summary>
        /// Invokes the handler on the <see cref="IRequestHandler"/>
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="requestContext"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext, IRequestContext requestContext)
        {
            // pass to API layer
            await RequestHandler.HandleRequest();

            // invoke next step in pipeline (if any)
            await Next(httpContext);
        }
    }
}