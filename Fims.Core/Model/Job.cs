using Newtonsoft.Json.Linq;

namespace Fims.Core.Model
{
    public class Job : Resource
    {
        public Job()
        {
        }

        public Job(JToken jobProfile, JToken jobInput, JToken asyncEndpointToken)
        {
            JobProfile = jobProfile;
            JobInput = jobInput;
            AsyncEndpointToken = asyncEndpointToken;

            JobStatus = "New";
            JobStatusReason = null;
            JobProcess = null;
            JobOutput = null;
        }

        public string JobStatus
        {
            get => GetString(nameof(JobStatus));
            set => Set(nameof(JobStatus), value);
        }

        public string JobStatusReason
        {
            get => GetString(nameof(JobStatusReason));
            set => Set(nameof(JobStatusReason), value);
        }

        public JToken JobProfile
        {
            get => Get(nameof(JobProfile));
            set => Set(nameof(JobProfile), value);
        }

        public JobProfile Profile => JobProfile.ToResource<JobProfile>();

        public JToken AsyncEndpointToken
        {
            get => Get(nameof(AsyncEndpoint));
            set => Set(nameof(AsyncEndpoint), value);
        }

        public AsyncEndpoint AsyncEndpoint => JobProfile.ToResource<AsyncEndpoint>();

        public string JobProcess
        {
            get => GetString(nameof(JobProcess));
            set => Set(nameof(JobProcess), value);
        }

        public JToken JobInput
        {
            get => Get(nameof(JobInput));
            set => Set(nameof(JobInput), value);
        }

        public JToken JobOutput
        {
            get => Get(nameof(JobOutput));
            set => Set(nameof(JobOutput), value);
        }
    }
}