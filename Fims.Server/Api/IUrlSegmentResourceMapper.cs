using System;

namespace Fims.Server.Api
{
    public interface IUrlSegmentResourceMapper
    {
        /// <summary>
        /// Maps a type name to a resource type
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="parentType"></param>
        /// <returns></returns>
        Type GetResourceType(string typeName, Type parentType = null);
    }
}