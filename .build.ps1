# Get version and docker tag
.version.ps1

# Publish terragate api
Write-Host "Publishing terragate..."
dotnet publish -nologo --configuration Release --runtime linux-x64 --no-self-contained src\Terragate.sln
if ($LASTEXITCODE -ne 0) {
    # Stop the script if dotnet publish fails
    return
}

# Build docker container
Write-Host "Building docker image $global:currentTag..."
docker build -t $global:latestTag -t $global:currentTag .
if ($LASTEXITCODE -eq 1) {
    # Check if docker services are running
    .docker.ps1
}

    # Run the latest version of the container
    .start.ps1
    $global:gitVersion = $null
