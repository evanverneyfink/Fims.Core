using System;
using Fims.Core.Model;
using Fims.Server.Business;

namespace Fims.Services.Jobs.WorkerFunctions
{
    public class WorkerFunctionInvocation : IResourceHandlerRegistration
    {
        public void Register(ResourceHandlerRegistryOptions opts)
        {
            opts.Register<Job>()
                .Register<JobProcess>()
                .Register<JobAssignment, WorkerFunctionJobResourceHandler>();
        }
    }
}