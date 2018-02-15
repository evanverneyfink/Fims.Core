using Newtonsoft.Json.Linq;

namespace Fims.Core.Model
{
    public class CaptureJob : Job
    {
        public CaptureJob()
        {
        }

        public CaptureJob(string jobProfileId, JObject jobInput, AsyncEndpoint asyncEndpoint)
            : base(jobProfileId, jobInput, asyncEndpoint)
        {
        }
    }
}