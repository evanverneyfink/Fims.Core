runtime = "dotnetcore2.0"

region = "us-east-1"

serviceName = "job-processor"
environmentName = "fims-cloud-test"
environmentType = "dev"

restApiHandler = "Fims.Aws.Services.Jobs.JobProcessor::Fims.Aws.Services.Jobs.JobProcessor.JobProcessorFunctions::Api"
restApiZipFile = "../../bin/Release/netcoreapp2.0/publish/lambda/functions.zip"