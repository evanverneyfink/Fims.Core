﻿using Fims.Core.Serialization;
using Fims.Server.Api;
using Fims.Services.Jobs.WorkerFunctions;

namespace Fims.Aws.ServiceBuilding
{
    public interface IFimsAwsWorkerService : IFimsService
    {
        /// <summary>
        /// Gets the worker
        /// </summary>
        IWorker Worker { get; }

        /// <summary>
        /// Gets the resource serializer
        /// </summary>
        IResourceSerializer ResourceSerializer { get; }
    }
}