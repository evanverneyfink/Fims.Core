using Newtonsoft.Json.Linq;

namespace Fims.Core.Model
{
    public class QaJob : Job
    {
        public QaJob()
        {
        }

        public QaJob(string jobProfileId, JObject jobInput, AsyncEndpoint asyncEndpoint)
            : base(jobProfileId, jobInput, asyncEndpoint)
        {
        }
    }
}