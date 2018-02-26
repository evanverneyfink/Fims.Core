using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Fims.Core.Model;
using Fims.Services.Files;

namespace Fims.Aws.S3
{
    public class S3FileStorage : IFileStorage
    {
        /// <summary>
        /// Gets the S3 client
        /// </summary>
        private IAmazonS3 S3 { get; } = new AmazonS3Client();

        /// <summary>
        /// Saves a file by doing a put to the S3 API
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="file"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        public async Task SaveFile(Locator locator, string file, string contents)
        {
            await S3.PutObjectAsync(new PutObjectRequest
            {
                BucketName = locator.S3Bucket(),
                Key = (locator.S3Key() ?? string.Empty) + file,
                ContentBody = contents
            });
        }
    }
}
