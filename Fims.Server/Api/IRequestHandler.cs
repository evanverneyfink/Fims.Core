using System.Threading.Tasks;

namespace Fims.Server.Api
{
    public interface IRequestHandler
    {
        /// <summary>
        /// Handles an HTTP request
        /// </summary>
        /// <returns></returns>
        Task<IResponse> HandleRequest();
    }
}