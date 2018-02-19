using Newtonsoft.Json.Linq;

namespace Fims.Core.Model
{
    public class QaJob : Job
    {
        public QaJob()
        {
        }

        public QaJob(JToken jobProfile, JToken jobInput, JToken asyncEndpointToken)
            : base(jobProfile, jobInput, asyncEndpointToken)
        {
        }
    }
}