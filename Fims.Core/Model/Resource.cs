using System;
using Fims.Core.JsonLd;
using Newtonsoft.Json.Linq;

namespace Fims.Core.Model
{
    public abstract class Resource : JObject
    {
        protected Resource(JObject jObj = null)
        {
            if (jObj != null)
                CopyFrom(jObj);
        }

        protected Resource(string type, JObject jObj = null)
            : this(jObj)
        {
            Context = Contexts.Default.Url;
            Type = type ?? GetType().Name;
        }

        public string Id
        {
            get => GetString(nameof(Id));
            set => Set(nameof(Id), value);
        }

        public JToken Context
        {
            get => base["@context"];
            set => base["@context"] = value;
        }

        public string Type
        {
            get => GetString(nameof(Type));
            set => Set(nameof(Type), value);
        }

        public DateTime? DateCreated
        {
            get => Get<DateTime>(nameof(DateCreated));
            set => Set(nameof(DateCreated), value);
        }

        public DateTime? DateModified
        {
            get => Get<DateTime>(nameof(DateModified));
            set => Set(nameof(DateModified), value);
        }

        protected JToken Get(string propName)
        {
            return base[propName.PascalCaseToCamelCase()];
        }

        protected T? Get<T>(string propName) where T : struct
        {
            return Get(propName)?.Value<T>();
        }

        protected string GetString(string propName)
        {
            return Get(propName)?.Value<string>();
        }

        protected void Set(string propName, JToken value)
        {
            base[propName.PascalCaseToCamelCase()] = value;
        }

        protected void CopyFrom(JObject jObj)
        {
            foreach (var prop in jObj.Properties())
                this[prop.Name] = prop.Value;
        }

        public static T From<T>(JObject jObj) where T : Resource, new()
        {
            var newObj = new T();
            newObj.CopyFrom(jObj);
            newObj.Type = typeof(T).Name;
            return newObj;
        }

        public static T FromId<T>(string id) where T : Resource, new()
        {
            return new T { Id = id };
        }

        public static T FromToken<T>(JToken token) where T : Resource, new()
        {
            if (token == null)
                return null;

            switch (token.Type)
            {
                case JTokenType.String:
                    return FromId<T>(token.Value<string>());
                case JTokenType.Object:
                    return From<T>((JObject)token);
                default:
                    return null;
            }
        }

        public static Resource FromToken(JToken token, Type type)
        {
            if (token == null)
                return null;

            if (!typeof(Resource).IsAssignableFrom(type))
                throw new Exception($"Cannot create object of type {type.Name} from JSON. Type must derive from type {typeof(Resource).Name}.");

            var ctor = type.GetConstructor(System.Type.EmptyTypes);
            if (ctor == null)
                throw new Exception($"Cannot create object of type {type.Name} from JSON. Type must have a public empty constructor.");

            var resource = (Resource)ctor.Invoke(new object[0]);

            switch (token.Type)
            {
                case JTokenType.String:
                    resource.Id = token.Value<string>();
                    break;
                case JTokenType.Object:
                    resource.CopyFrom((JObject)token);
                    break;
                default:
                    return null;
            }

            return resource;
        }
    }
}