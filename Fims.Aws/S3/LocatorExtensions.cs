using Fims.Core.Model;
using Newtonsoft.Json.Linq;

namespace Fims.Aws.S3
{
    public static class LocatorExtensions
    {
        /// <summary>
        /// Gets the bucket for an S3 locator
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        public static string S3Bucket(this Locator locator)
        {
            return locator["awsS3Bucket"]?.Value<string>();
        }

        /// <summary>
        /// Gets the key for an S3 locator
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        public static string S3Key(this Locator locator)
        {
            return locator["awsS3Key"]?.Value<string>();
        }
    }
}