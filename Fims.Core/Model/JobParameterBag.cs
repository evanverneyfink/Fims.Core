using Newtonsoft.Json.Linq;

namespace Fims.Core.Model
{
    public class JobParameterBag : Resource
    {
        public JobParameterBag()
        {
        }

        public JobParameterBag(JObject parameters)
            : base(null, parameters)
        {

        }
    }
}