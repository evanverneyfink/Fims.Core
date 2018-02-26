runtime = "dotnetcore2.0"

region = "us-east-1"

serviceName = "mediainfo-service"
environmentName = "fims-cloud-test"
environmentType = "dev"

restApiHandler = "Fims.Aws.Services.Ame.MediaInfo::Fims.Aws.Services.Ame.MediaInfo.MediaInfoFunctions::JobApi"
restApiZipFile = "../../bin/Release/netcoreapp2.0/publish/lambda/functions.zip"

workerHandler = "Fims.Aws.Services.Ame.MediaInfo::Fims.Aws.Services.Ame.MediaInfo.MediaInfoFunctions::Worker"
workerZipFile = "../../bin/Release/netcoreapp2.0/publish/lambda/functions.zip"

serviceRegistryUrl = "https://dpzr6corrg.execute-api.us-east-1.amazonaws.com/dev"