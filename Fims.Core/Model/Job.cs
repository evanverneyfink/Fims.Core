using Newtonsoft.Json.Linq;

namespace Fims.Core.Model
{
    public abstract class Job : JobStatusObject
    {
        protected Job()
        {
        }

        protected Job(string jobProfileId, JObject jobInput, AsyncEndpoint asyncEndpoint)
        {
            JobProfileId = jobProfileId;
            JobInput = jobInput;
            AsyncEndpoint = asyncEndpoint;
        }

        public string JobProfileId { get; set; }

        public JObject JobInput { get; set; }

        public AsyncEndpoint AsyncEndpoint { get; set; }

        public string JobProcessId { get; set; }
        
        public JObject JobOutput { get; set; }
    }
}