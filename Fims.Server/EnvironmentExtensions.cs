namespace Fims.Server
{
    public static class EnvironmentExtensions
    {
        /// <summary>
        /// Checks if a resource descriptor is for a local resource
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="resourceDescriptor"></param>
        /// <returns></returns>
        public static bool IsLocalResource(this IEnvironment environment, ResourceDescriptor resourceDescriptor)
        {
            return (resourceDescriptor?.Url?.StartsWith(environment.PublicUrl)).GetValueOrDefault();
        }
    }
}