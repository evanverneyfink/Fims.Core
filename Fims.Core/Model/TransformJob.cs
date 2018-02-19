using Newtonsoft.Json.Linq;

namespace Fims.Core.Model
{
    public class TransformJob : Job
    {
        public TransformJob()
        {
        }

        public TransformJob(JToken jobProfile, JToken jobInput, JToken asyncEndpointToken)
            : base(jobProfile, jobInput, asyncEndpointToken)
        {
        }
    }
}