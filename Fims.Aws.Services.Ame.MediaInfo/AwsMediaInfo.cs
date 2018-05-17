using Fims.Aws.ServiceBuilding;
using Fims.Services.Ame.MediaInfo;

namespace Fims.Aws.Services.Ame.MediaInfo
{
    public static class AwsMediaInfo
    {
        public static FimsAwsServiceBuilder AddAwsMediaInfo(this FimsAwsServiceBuilder serviceBuilder)
        {
            return serviceBuilder.With(services => services.AddMediaInfo<S3UrlProvider, LambdaProcessLocator>());
        }
    }
}