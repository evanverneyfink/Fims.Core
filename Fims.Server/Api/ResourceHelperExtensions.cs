using System;
using Fims.Core;

namespace Fims.Server.Api
{
    public static class ResourceHelperExtensions
    {
        /// <summary>
        /// Gets a resource descriptor from a uri
        /// </summary>
        /// <param name="resourceDescriptorHelper"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static ResourceDescriptor GetResourceDescriptor(this IResourceDescriptorHelper resourceDescriptorHelper, Uri uri)
        {
            return resourceDescriptorHelper.GetResourceDescriptor(uri.AbsolutePath, uri.ToString().Substring(0, uri.AbsolutePath.Length));
        }
    }
}