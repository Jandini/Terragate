# Get version and docker tag
.version.ps1

# Publish terragate api
Write-Host "Publishing terragate..."
dotnet publish -nologo --configuration Release --runtime linux-x64 --no-self-contained src\Terragate.sln
if ($LASTEXITCODE -ne 0) {
    # Stop the script if dotnet publish fails
    return
}

# Build docker image
.build.ps1 

# Run the latest version of the container
.start.ps1
$global:gitVersion = $null
