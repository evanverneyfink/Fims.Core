# build code and package into zip for lambda
dotnet publish .. -c=Release
New-Item -ItemType Directory -Force -Path ..\bin\Release\netstandard2.0\publish\functions
Compress-Archive -Path ..\bin\Release\netstandard2.0\publish\* -DestinationPath ..\bin\Release\netstandard2.0\publish\functions\functions.zip -Force

# run terraform to deploy to AWS
cd terraform
terraform init -input=false
terraform apply -auto-approve -var-file="private.tfvars" -var-file="public.tfvars"
cd ..