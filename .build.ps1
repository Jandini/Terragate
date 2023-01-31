# NOTE: This scipt does not guarantee to include latest build. 
#       Use publish to run dotnet publish and docker build.

# Get version and docker tag
.version.ps1

# Run publish if publish folder does not exist. 
if (!(Test-Path ".\src\Terragate.Api\bin\Release\net7.0\linux-x64\publish")) {
    .publish.ps1
}

# Build docker container
Write-Host "Building docker image $global:currentTag..."
docker build -t $global:currentTag .  --build-arg ADD_CA_CERTS=${env:ADD_CA_CERTS}
if ($LASTEXITCODE -eq 1) {
    # Check if docker services are running
    .docker.ps1
}