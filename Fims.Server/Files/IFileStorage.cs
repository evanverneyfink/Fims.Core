using System.Threading.Tasks;
using Fims.Core.Model;

namespace Fims.Server.Files
{
    public interface IFileStorage
    {
        /// <summary>
        /// Writes text to a file
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="fileName"></param>
        /// <param name="contents"></param>
        Task<Locator> WriteTextToFile(Locator locator, string fileName, string contents);
    }
}