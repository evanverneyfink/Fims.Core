using System;
using System.IO;
using System.Threading.Tasks;
using Fims.Core.Model;

namespace Fims.Server.Files
{
    public class LocalFileStorage : IFileStorage
    {
        /// <summary>
        /// Saves a file to storage
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="fileName"></param>
        /// <param name="contents"></param>
        public Task SaveFile(Locator locator, string fileName, string contents)
        {
            if (!(locator is LocalLocator localLocator))
                throw new Exception("Locator is not for a local file.");

            // build path to output folder
            var filePath = Path.Combine(localLocator.FolderPath, (localLocator.FileName ?? String.Empty) + fileName);

            // write file
            File.WriteAllText(filePath, contents);

            return Task.CompletedTask;
        }
    }
}