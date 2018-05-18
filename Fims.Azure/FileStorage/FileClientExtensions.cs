using System.Linq;
using Fims.Core;
using Microsoft.WindowsAzure.Storage.File;

namespace Fims.Azure.FileStorage
{
    public static class FileClientExtensions
    {
        /// <summary>
        /// Gets a file from a <see cref="CloudFileClient"/>
        /// </summary>
        /// <param name="fileClient"></param>
        /// <param name="locator"></param>
        /// <returns></returns>
        public static CloudFile GetFile(this CloudFileClient fileClient, AzureFileStorageLocator locator)
            => locator.Directory.SplitOn("/")
                      .Aggregate(fileClient.GetShareReference(locator.Share).GetRootDirectoryReference(),
                                 (current, dirPart) => current.GetDirectoryReference(dirPart))
                      .GetFileReference(locator.FileName);
    }
}