runtime = "dotnetcore2.0"

region = "us-east-1"

serviceName = "service-registry"
environmentName = "fims-cloud-test"
environmentType = "dev"

restApiHandler = "Fims.Aws.Services.ServiceRegistry::Fims.Aws.Services.ServiceRegistry.ServiceRegistryFunctions::Api"
restApiZipFile = "../../bin/Release/netcoreapp2.0/publish/lambda/functions.zip"