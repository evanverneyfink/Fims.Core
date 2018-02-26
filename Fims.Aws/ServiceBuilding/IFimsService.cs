using System;
using Fims.Server;

namespace Fims.Aws.ServiceBuilding
{
    public interface IFimsService : IDisposable
    {
        /// <summary>
        /// Gets the logger
        /// </summary>
        ILogger Logger { get; }
    }
}