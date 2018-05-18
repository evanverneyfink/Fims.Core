using Fims.Core.Model;

namespace Fims.Aws.S3
{
    public class AwsS3Locator : Locator
    {
        /// <summary>
        /// Gets the AWS S3 bucket
        /// </summary>
        public string AwsS3Bucket { get; set; }

        /// <summary>
        /// Gets the AWS S3 key
        /// </summary>
        public string AwsS3Key { get; set; }
    }
}