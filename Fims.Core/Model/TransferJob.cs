using Newtonsoft.Json.Linq;

namespace Fims.Core.Model
{
    public class TransferJob : Job
    {
        public TransferJob()
        {
        }

        public TransferJob(string jobProfileId, JObject jobInput, AsyncEndpoint asyncEndpoint)
            : base(jobProfileId, jobInput, asyncEndpoint)
        {
        }
    }
}