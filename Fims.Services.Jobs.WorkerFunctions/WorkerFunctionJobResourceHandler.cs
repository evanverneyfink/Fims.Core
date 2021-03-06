﻿using System.Threading.Tasks;
using Fims.Core;
using Fims.Core.Model;
using Fims.Server;
using Fims.Server.Api;
using Fims.Server.Business;
using Fims.Server.Data;
using Fims.Server.Environment;

namespace Fims.Services.Jobs.WorkerFunctions
{
    public class WorkerFunctionJobResourceHandler : ResourceHandler<JobAssignment>
    {
        /// <summary>
        /// Instantiates a <see cref="WorkerFunctionJobResourceHandler"/>
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="dataHandler"></param>
        /// <param name="resourceDescriptorHelper"></param>
        /// <param name="environment"></param>
        /// <param name="workerFunctionInvoker"></param>
        public WorkerFunctionJobResourceHandler(ILogger logger,
                                                IEnvironment environment,
                                                IResourceDataHandler dataHandler,
                                                IResourceDescriptorHelper resourceDescriptorHelper,
                                                IWorkerFunctionInvoker workerFunctionInvoker)
            : base(logger, environment, dataHandler, resourceDescriptorHelper)
        {
            WorkerFunctionInvoker = workerFunctionInvoker;
        }

        /// <summary>
        /// Gets the worker function invoker
        /// </summary>
        private IWorkerFunctionInvoker WorkerFunctionInvoker { get; }

        /// <summary>
        /// Gets a resource of type <see cref="JobAssignment"/> by its ID
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public override async Task<JobAssignment> Create(ResourceDescriptor resourceDescriptor, JobAssignment resource)
        {
            // create the job assinment in the data layer
            var jobAssignment = await base.Create(resourceDescriptor, resource);

            // invoke worker
            await WorkerFunctionInvoker.Invoke(Environment.WorkerFunctionName(), Environment, resource);

            // return the newly-created job assignment
            return jobAssignment;
        }
    }
}
