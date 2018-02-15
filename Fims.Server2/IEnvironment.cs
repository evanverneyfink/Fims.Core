namespace Fims.Server
{
    public interface IEnvironment
    {
        /// <summary>
        /// Gets the root path of the url
        /// </summary>
        string RootPath { get; }

        /// <summary>
        /// Gets the base public url for the server
        /// </summary>
        string PublicUrl { get; }
    }
}