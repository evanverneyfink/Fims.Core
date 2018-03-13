using System;
using Fims.Core.Model;

namespace Fims.Core
{
    public static class ResourceTypeExtensions
    {
        public static Type ToResourceType(this string name)
        {
            // try with the name as-is at first
            var typeValue = Type.GetType(name);

            // try assuming the core namespace
            if (typeValue == null)
                typeValue = Type.GetType($"{typeof(Resource).Namespace}.{name}");

            // try assuming the core assembly-qualified name
            if (typeValue == null)
                typeValue = Type.GetType($"{typeof(Resource).Assembly.GetName().Name}, {typeof(Resource).Namespace}.{name}");

            return typeValue;
        }

        public static string ToResourceTypeName(this Type type)
        {
            return type.Assembly.FullName == typeof(Resource).Assembly.FullName ? type.Name : type.AssemblyQualifiedName;
        }
    }
}
