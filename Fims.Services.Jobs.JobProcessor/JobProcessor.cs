using System;
using Fims.Core.Model;
using Fims.Server.Business;

namespace Fims.Services.Jobs.JobProcessor
{
    public class JobProcessor : IResourceHandlerRegistration
    {
        public void Register(ResourceHandlerRegistryOptions opts)
        {
            opts.Register<JobProcess, JobProcessorResourceHandler>();
        }
    }
}
