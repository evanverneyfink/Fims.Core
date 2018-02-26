using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Fims.Core.Model
{
    public static class JsonExtensions
    {
        public static string ToId(this JToken token)
        {
            if (token == null)
                return null;

            switch (token.Type)
            {
                case JTokenType.String:
                    return token.Value<string>();
                case JTokenType.Object:
                    return token.Value<JObject>()?["id"]?.Value<string>();
                default:
                    return null;
            }
        }

        public static T ToResource<T>(this JToken token) where T : Resource, new()
        {
            return Resource.FromToken<T>(token);
        }

        public static ICollection<T> ToResourceCollection<T>(this JToken token) where T : Resource, new()
        {
            if (token == null)
                return null;

            switch (token.Type)
            {
                case JTokenType.String:
                case JTokenType.Object:
                    return new List<T> {Resource.FromToken<T>(token)};
                case JTokenType.Array:
                    return ((JArray)token).Select(Resource.FromToken<T>).ToList();
                default:
                    return null;
            }
        }
    }
}