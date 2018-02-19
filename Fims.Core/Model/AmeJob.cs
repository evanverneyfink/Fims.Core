using Newtonsoft.Json.Linq;

namespace Fims.Core.Model
{
    public class AmeJob : Job
    {
        public AmeJob()
        {
        }

        public AmeJob(JToken jobProfile, JToken jobInput, AsyncEndpoint asyncEndpointToken)
            : base(jobProfile, jobInput, asyncEndpointToken)
        {
        }
    }
}