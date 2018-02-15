using Newtonsoft.Json.Linq;

namespace Fims.Core.Model
{
    public class TransformJob : Job
    {
        public TransformJob()
        {
        }

        public TransformJob(string jobProfileId, JObject jobInput, AsyncEndpoint asyncEndpoint)
            : base(jobProfileId, jobInput, asyncEndpoint)
        {
        }
    }
}