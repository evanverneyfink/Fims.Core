using System;
using Fims.Core;
using Newtonsoft.Json;

namespace Fims.Json
{
    public class TypeConverter : JsonConverter<Type>
    {
        public override void WriteJson(JsonWriter writer, Type value, JsonSerializer serializer)
            => writer.WriteValue(value.Name);

        public override Type ReadJson(JsonReader reader, Type objectType, Type existingValue, bool hasExistingValue, JsonSerializer serializer)
            => reader.Value?.ToString()?.ToResourceType();
    }
}