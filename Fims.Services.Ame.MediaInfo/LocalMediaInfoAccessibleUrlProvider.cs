using System.IO;
using System.Threading.Tasks;
using Fims.Core.Model;
using Fims.Server.Files;

namespace Fims.Services.Ame.MediaInfo
{
    internal class LocalMediaInfoAccessibleUrlProvider : IMediaInfoAccessibleUrlProvider
    {
        /// <summary>
        /// Gets a url for a given <see cref="Locator"/> that's accessible by MediaInfo on the local machine
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        public Task<string> GetMediaInfoAccessibleUrl(Locator locator)
        {
            return Task.FromResult(locator is LocalLocator local ? Path.Combine(local.FolderPath, local.FileName) : null);
        }
    }
}