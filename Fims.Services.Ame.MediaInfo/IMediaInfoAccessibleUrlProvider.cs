using System.Threading.Tasks;
using Fims.Core.Model;

namespace Fims.Services.Ame.MediaInfo
{
    public interface IMediaInfoAccessibleUrlProvider
    {
        /// <summary>
        /// Gets a url for a given <see cref="Locator"/> that's accessible by MediaInfo in the given context
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        Task<string> GetMediaInfoAccessibleUrl(Locator locator);
    }
}