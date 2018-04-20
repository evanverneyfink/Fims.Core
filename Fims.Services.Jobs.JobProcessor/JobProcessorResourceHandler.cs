﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Fims.Core;
using Fims.Core.Jobs;
using Fims.Core.Model;
using Fims.Server;
using Fims.Server.Api;
using Fims.Server.Business;
using Fims.Server.Data;

namespace Fims.Services.Jobs.JobProcessor
{
    public class JobProcessorResourceHandler : ResourceHandler<JobProcess>
    {
        /// <summary>
        /// Instantiates a <see cref="JobProcessorResourceHandler"/>
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="dataHandler"></param>
        /// <param name="resourceDescriptorHelper"></param>
        /// <param name="environment"></param>
        public JobProcessorResourceHandler(ILogger logger,
                                           IEnvironment environment,
                                           IResourceDataHandler dataHandler,
                                           IResourceDescriptorHelper resourceDescriptorHelper)
            : base(logger, environment, dataHandler, resourceDescriptorHelper)
        {
        }

        /// <summary>
        /// Handles creation of a job
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <param name="jobProcess"></param>
        /// <returns></returns>
        public override async Task<JobProcess> Create(ResourceDescriptor resourceDescriptor, JobProcess jobProcess)
        {
            // ensure status is set to New
            jobProcess.JobProcessStatus = "New";

            // call base first
            jobProcess = await base.Create(resourceDescriptor, jobProcess);

            // retrieve the job data
            var job = await DataHandler.Get<Job>(jobProcess.Job.Id);
            if (job != null)
            {
                // validate the job before processing
                job.Validate();

                // get all services
                var services = await DataHandler.Query<Service>(
                                   ResourceDescriptor.FromUrl<Service>(Environment.ServiceRegistryUrl().TrimEnd('/') + "/Services"));

                // find first service that can accept the job type and that has a job assignment endpoint
                var serviceResource =
                    services
                        .Where(s => s.CanAcceptJob(job))
                        .SelectMany(s => s.HasResource.Select(r => new {Service = s, Resource = r}))
                        .FirstOrDefault(
                            sr => sr.Resource.ResourceType == nameof(JobAssignment) && !string.IsNullOrWhiteSpace(sr.Resource.HttpEndpoint));

                // check that we found an acceptable endpoint
                if (serviceResource != null)
                {
                    try
                    {
                        // send the job to the job assignment endpoint
                        var jobAssignment =
                            await DataHandler.Create(ResourceDescriptor.FromUrl<JobAssignment>(serviceResource.Resource.HttpEndpoint),
                                                     new JobAssignment {JobProcess = jobProcess.Id });

                        // set assignment back on the job process
                        jobProcess.JobAssignment = jobAssignment.Id;

                        // set status to Running
                        jobProcess.JobProcessStatus = job.JobStatus = "Running";
                        jobProcess.JobStart = DateTime.UtcNow;
                    }
                    catch
                    {
                        jobProcess.JobProcessStatus = "Failed";
                        jobProcess.JobProcessStatusReason = $"Service '{serviceResource.Service.Label}' failed to accept JobAssignment";
                        job.JobStatus = "Failed";
                        job.JobStatusReason = $"Service '{serviceResource.Service.Label}' failed to accept JobAssignment";
                    }
                }
                else
                {
                    jobProcess.JobProcessStatus = "Failed";
                    jobProcess.JobProcessStatusReason = "No accepting service available";
                    job.JobStatus = "Failed";
                    job.JobStatusReason = "No accepting service available";
                }

                // update the job
                try
                {
                    await DataHandler.Update(ResourceDescriptor.FromUrl<Job>(job.Id), job);
                }
                catch
                {
                    jobProcess.JobProcessStatus = "Failed";
                    jobProcess.JobProcessStatusReason = $"Unable to update job '{job.Id}'";
                }

                // hit the async callback endpoint
                if (jobProcess.JobProcessStatus == "Failed" && job.AsyncEndpoint != null && !string.IsNullOrWhiteSpace(job.AsyncEndpoint.AsyncFailure))
                    await DataHandler.Get<Resource>(job.AsyncEndpoint.AsyncFailure);
            }
            else
            {
                jobProcess.JobProcessStatus = "Failed";
                jobProcess.JobProcessStatusReason = $"Unable to retrieve job '{jobProcess.Job}'";
            }

            // update the JobProcess record and return it
            return await Update(resourceDescriptor, jobProcess);
        }

        /// <summary>
        /// Handle update of a job
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <param name="resource"></param>
        public override async Task<JobProcess> Update(ResourceDescriptor resourceDescriptor, JobProcess resource)
        {
            var jobProcess = await DataHandler.Get<JobProcess>(resourceDescriptor);

            // validate the update
            if (jobProcess.JobProcessStatus == "New" && !new[] {"Running", "Completed", "Failed"}.Contains(resource.JobProcessStatus) ||
                jobProcess.JobProcessStatus == "Running" && !new[] {"Completed", "Failed"}.Contains(resource.JobProcessStatus))
                throw new Exception($"Cannot change status of job process from '{jobProcess.JobProcessStatus}' to '{resource.JobProcessStatus}'");

            jobProcess.JobProcessStatus = resource.JobProcessStatus;

            var job = await DataHandler.Get<Job>(jobProcess.Job.Id);

            // update job
            job.JobStatus = jobProcess.JobProcessStatus;
            job.JobStatusReason = jobProcess.JobProcessStatusReason;

            // update output from job assignment
            if (!string.IsNullOrWhiteSpace(jobProcess.JobAssignment))
            {
                var jobAssignment = await DataHandler.Get<JobAssignment>(jobProcess.JobAssignment);
                job.JobOutput = jobAssignment.JobOutput;
            }

            await DataHandler.Update(job);

            // clear the job assignment
            jobProcess.JobAssignment = null;

            var updated = await base.Update(resourceDescriptor, jobProcess);

            // send notification to async endpoint if provided
            if (job.AsyncEndpoint != null)
            {
                if (job.JobStatus == "Completed" && !string.IsNullOrWhiteSpace(job.AsyncEndpoint.AsyncSuccess))
                    await DataHandler.Get(job.AsyncEndpoint.AsyncSuccess);
                else if (job.JobStatus == "Failed" && !string.IsNullOrWhiteSpace(job.AsyncEndpoint.AsyncFailure))
                    await DataHandler.Get(job.AsyncEndpoint.AsyncFailure);
            }

            return updated;
        }
    }
}