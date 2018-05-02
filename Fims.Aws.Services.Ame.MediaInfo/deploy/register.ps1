# get terraform outputs
cd terraform
$apiUrl = terraform output restServiceUrl | Out-String
$serviceRegistryUrl = terraform output serviceRegistryUrl | Out-String
$apiUrl = $apiUrl -replace "`n","" -replace "`r",""
$serviceRegistryUrl = $serviceRegistryUrl -replace "`n","" -replace "`r",""
cd ..

# read terraform output and store it into the service.json
$serviceJson = (Get-Content ..\service.json | Out-String)
$serviceJson = $serviceJson.Replace("{{{serviceUrl}}}", $apiUrl)
$service = ConvertFrom-Json -InputObject $serviceJson

# get all the available job profiles from the service registry
$jobProfilesJson = Invoke-WebRequest -Method GET -Uri $serviceRegistryUrl"/JobProfiles"
Write-Host "Got job profiles JSON: "$jobProfilesJson
$jobProfiles = ConvertFrom-Json $jobProfilesJson

# keep a collection of available profiles
$jobProfileUrls = New-Object System.Collections.ArrayList

Foreach ($tmp in $jobProfiles)
{
	Write-Host "Found JobProfile: "$tmp.label
}

# create or update each job profile
Foreach ($jobProfile in $service.acceptsJobProfile)
{
	$existingJobProfile = Where-Object -InputObject $jobProfiles -Property label -eq $jobProfile.label
	
	# convert the job profile back to JSON in order to send it as the body of a request
	$jobProfileJson = ConvertTo-Json $jobProfile

	# if there's no existing profile with the given label, we need to create it
	# otherwise we need to update it
	If ($existingJobProfile -eq $null)
	{
		# do a POST to create the new job profile and read the JSON response as an object
		$jobProfileResponse = Invoke-RestMethod -Method POST -Uri $serviceRegistryUrl"/JobProfiles" -Body $jobProfileJson
		# Write-Host "Got newly-created job profile JSON: "$jobProfileResponseJson
		# $jobProfileResponse = ConvertFrom-Json $jobProfileResponseJson

		# store the url of the new job profile
		$jobProfileUrls.Add($jobProfileResponse.id)
	}
	Else
	{
		# do a PUT to update the existing job profile
		Invoke-RestMethod -Method PUT -Uri $existingJobProfile.id -Body $jobProfileJson

		# store the url of the existing job profile
		$jobProfileUrls.Add($existingJobProfile.id)
	}
}

# replace job profiles objects with links
$service.acceptsJobProfile = $jobProfileUrls.ToArray()
Write-Host $service

# convert the service back to JSON now that it has been updated
$serviceJson = ConvertTo-Json $service

# get existing services
$servicesJson = Invoke-WebRequest -Method GET -Uri $serviceRegistryUrl"/Services"
$services = ConvertFrom-Json $servicesJson

# check if the service already exists
$existingService = Where-Object -InputObject $services -Property label -eq $service.label

# register the service with the service registry
If ($existingService -eq $null)
{
	Invoke-RestMethod -Method POST -Uri $serviceRegistryUrl"/Services" -Body $serviceJson
}
Else
{
	Invoke-RestMethod -Method PUT -Uri $existingService.id -Body $serviceJson
}