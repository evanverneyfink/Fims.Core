using System;
using System.Linq;
using System.Reflection;

namespace Fims.Azure.DependencyInjection
{
    internal static class ServiceProviderBuilderHelper
    {
        internal static IServiceProviderBuilder GetBuilder(Assembly assembly)
        {
            var builderType = typeof(IServiceProviderBuilder);
            var builder = assembly.GetExportedTypes().Single(t => builderType.IsAssignableFrom(t));
            return (IServiceProviderBuilder)Activator.CreateInstance(builder);
        }
    }
}