using Newtonsoft.Json.Linq;

namespace Fims.Core.Model
{
    public class JobParameterBag : FimsObject
    {
        public JobParameterBag(JObject parameters)
            : base(null, parameters)
        {

        }
    }
}