using Fims.Server.Api;

namespace Fims.Aws.ServiceBuilding
{
    public interface IFimsAwsResourceApi : IFimsService
    {
        /// <summary>
        /// Gets the request handler
        /// </summary>
        IRequestHandler RequestHandler { get; }
    }
}