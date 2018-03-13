using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Fims.Server
{
    public class JsonContent : StringContent
    {
        public JsonContent(object obj, JsonSerializerSettings settings = null)
            : base(JsonConvert.SerializeObject(obj, settings ?? new JsonSerializerSettings()), Encoding.UTF8, "application/json")
        {
        }
    }
}