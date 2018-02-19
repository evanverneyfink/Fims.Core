using Newtonsoft.Json.Linq;

namespace Fims.Core.Model
{
    public class Locator : Resource
    {
        public Locator()
        {
        }

        public Locator(JObject jObject)
        {
            CopyFrom(jObject);
        }
    }
}