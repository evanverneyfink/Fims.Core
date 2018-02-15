namespace Fims.Server.Api
{
    public interface IResourceUrlParser
    {
        /// <summary>
        /// Gets a resource descriptor from the path of a resource
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        ResourceDescriptor GetResourceDescriptor(string path);
    }
}