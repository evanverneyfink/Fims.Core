using System;
using Newtonsoft.Json.Linq;

namespace Fims.Core.Model
{
    public class JobProcess : Resource
    {
        public JobProcess()
        {
        }

        public JobProcess(JToken job)
            : this()
        {
            JobToken = job;

            JobAssignmentToken = null;
            JobProcessStatus = "New";
            JobProcessStatusReason = null;
            JobStart = null;
            JobDuration = null;
            JobEnd = null;
        }

        public JToken JobToken
        {
            get => Get(nameof(Job));
            set => Set(nameof(Job), value);
        }

        public Job Job => JobToken.ToResource<Job>();

        public JToken JobAssignmentToken
        {
            get => Get(nameof(JobAssignmentToken));
            set => Set(nameof(JobAssignmentToken), value);
        }

        public JobAssignment JobAssignment => JobAssignmentToken.ToResource<JobAssignment>();

        public JToken JobProcessStatus
        {
            get => Get(nameof(JobProcessStatus));
            set => Set(nameof(JobProcessStatus), value);
        }

        public JobProcessStatus Status => JobProcessStatus.ToResource<JobProcessStatus>();

        public string JobProcessStatusReason
        {
            get => GetString(nameof(JobProcessStatusReason));
            set => Set(nameof(JobProcessStatusReason), value);
        }

        public DateTime? JobStart
        {
            get => Get<DateTime>(nameof(JobStart));
            set => Set(nameof(JobStart), value);
        }

        public TimeSpan? JobDuration
        {
            get => Get<TimeSpan>(nameof(JobDuration));
            set => Set(nameof(JobDuration), value);
        }

        public DateTime? JobEnd
        {
            get => Get<DateTime>(nameof(JobEnd));
            set => Set(nameof(JobEnd), value);
        }
    }
}