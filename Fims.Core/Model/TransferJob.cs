using Newtonsoft.Json.Linq;

namespace Fims.Core.Model
{
    public class TransferJob : Job
    {
        public TransferJob()
        {
        }

        public TransferJob(JToken jobProfile, JToken jobInput, JToken asyncEndpointToken)
            : base(jobProfile, jobInput, asyncEndpointToken)
        {
        }
    }
}