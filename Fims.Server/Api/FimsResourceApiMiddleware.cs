using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Fims.Server.Api
{
    public class FimsResourceApiMiddleware
    {
        /// <summary>
        /// Instantiates a <see cref="FimsResourceApiMiddleware"/>
        /// </summary>
        /// <param name="next"></param>
        public FimsResourceApiMiddleware(RequestDelegate next)
        {
            Next = next;
        }

        /// <summary>
        /// Gets the next step in the pipeline
        /// </summary>
        private RequestDelegate Next { get; }

        /// <summary>
        /// Invokes the handler on the <see cref="IRequestHandler"/>
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="requestHandler"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext, IRequestHandler requestHandler)
        {
            // invoke next step in pipeline (if any)
            await Next(httpContext);

            // pass to API layer
            await requestHandler.HandleRequest();
        }
    }
}