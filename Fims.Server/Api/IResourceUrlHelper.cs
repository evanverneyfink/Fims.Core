namespace Fims.Server.Api
{
    public interface IResourceUrlHelper
    {
        /// <summary>
        /// Gets a resource descriptor from the path of a resource
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        ResourceDescriptor GetResourceDescriptor(string path);

        /// <summary>
        /// Gets the path to a resource
        /// </summary>
        /// <param name="resourceDescriptor"></param>
        /// <returns></returns>
        string GetUrlPath(ResourceDescriptor resourceDescriptor);
    }
}