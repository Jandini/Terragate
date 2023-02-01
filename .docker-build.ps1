# NOTE: This scipt does not guarantee to include latest build. 
#       Use publish to run dotnet publish and docker build.

# Get version and docker tag
.git-version.ps1

# Run publish if publish folder does not exist. 
if (!(Test-Path ".\src\Terragate.Api\bin\Release\net7.0\linux-x64\publish")) {
    .publish.ps1
}

$dockerBuild = { 
	Write-Host "Running docker build for image $global:currentTag..."
	docker build -t $global:currentTag . 
}

# Build docker container
& $dockerBuild 
if ($LASTEXITCODE -eq 1) {    
    # Ensure docker service is running
    .docker-service.ps1 
    
    # Try to build container again
    & $dockerBuild
}