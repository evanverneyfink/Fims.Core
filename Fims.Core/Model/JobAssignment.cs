using Newtonsoft.Json.Linq;

namespace Fims.Core.Model
{
    public class JobAssignment : Resource
    {
        public JobAssignment()
        {
        }

        public JobAssignment(JToken jobProcess)
        {
            JobProcessToken = jobProcess;
            JobProcessStatus = "New";
            JobProcessStatusReason = null;
        }

        public JToken JobProcessToken
        {
            get => Get(nameof(JobProcess));
            set => Set(nameof(JobProcess), value);
        }

        public JobProcess JobProcess => JobProcessToken.ToResource<JobProcess>();

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

        public JToken JobOutput
        {
            get => Get(nameof(JobOutput));
            set => Set(nameof(JobOutput), value);
        }
    }
}