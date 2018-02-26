# build code and package into zip for lambda
dotnet publish .. -c=Release
New-Item -ItemType Directory -Force -Path ..\bin\Release\netcoreapp2.0\publish\lambda
Compress-Archive -Path ..\bin\Release\netcoreapp2.0\publish\* -DestinationPath ..\bin\Release\netcoreapp2.0\publish\lambda\functions.zip -Force

# run terraform to deploy to AWS
cd terraform
terraform init -input=false
terraform apply -auto-approve -var-file="private.tfvars" -var-file="public.tfvars"
$apiUrl = terraform output restServiceUrl | Out-String
$serviceRegistryUrl = terraform output serviceRegistryUrl | Out-String
$apiUrl = $apiUrl -replace "`n","" -replace "`r",""
$serviceRegistryUrl = $serviceRegistryUrl -replace "`n","" -replace "`r",""
cd ..

# read terraform output and store it into the service.json
$serviceJson = (Get-Content ..\service.json | Out-String)
$serviceJson = $serviceJson.Replace("{{{serviceUrl}}}", $apiUrl)

# register the service with the service registry
Invoke-RestMethod -Method POST -Uri $serviceRegistryUrl"/Services" -Body $serviceJson