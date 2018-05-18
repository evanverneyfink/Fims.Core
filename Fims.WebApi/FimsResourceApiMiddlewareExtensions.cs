using Microsoft.AspNetCore.Builder;

namespace Fims.WebApi
{
    public static class FimsResourceApiMiddlewareExtensions
    {
        /// <summary>
        /// Uses FIMS API middleware
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseFimsWebApi(this IApplicationBuilder app)
        {
            return app.UseMiddleware<FimsResourceApiMiddleware>();
        }
    }
}