using System.Threading.Tasks;
using Fims.Core.Model;

namespace Fims.Services.Files
{
    public interface IFileStorage
    {
        /// <summary>
        /// Saves a file to storage
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="file"></param>
        /// <param name="contents"></param>
        Task SaveFile(Locator locator, string file, string contents);
    }
}