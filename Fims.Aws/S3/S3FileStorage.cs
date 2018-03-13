using System;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Fims.Core.Model;
using Fims.Server.Files;

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
        /// <param name="fileName"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        public async Task SaveFile(Locator locator, string fileName, string contents)
        {
            if (!(locator is AwsS3Locator s3Locator))
                throw new Exception("Locator must be an AWS S3 locator.");

            await S3.PutObjectAsync(new PutObjectRequest
            {
                BucketName = s3Locator.AwsS3Bucket,
                Key = (s3Locator.AwsS3Key ?? string.Empty) + fileName,
                ContentBody = contents
            });
        }
    }
}
