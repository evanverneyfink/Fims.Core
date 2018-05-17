runtime = "dotnetcore2.0"

region = "us-east-1"

serviceName = "job-repository"
environmentName = "fims-cloud-test"
environmentType = "dev"

restApiHandler = "Fims.Aws.Services.Jobs.JobRepository::Fims.Aws.Services.Jobs.JobRepository.JobRepositoryFunctions::Api"
restApiZipFile = "../../bin/Release/netcoreapp2.0/publish/lambda/functions.zip"

serviceRegistryUrl = "https://dpzr6corrg.execute-api.us-east-1.amazonaws.com/dev"