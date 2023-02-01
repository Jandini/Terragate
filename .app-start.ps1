# Get version and docker tag
.git-version.ps1

# Publish terragate api
Write-Host "Running dotnet publish..."
dotnet publish -nologo --configuration Release --runtime linux-x64 --no-self-contained -p:OutputType=Exe -p:DebugOutput=None src\Terragate.sln

if ($LASTEXITCODE -ne 0) {
    # Stop the script if dotnet publish fails
    return
}

# Build docker image
.docker-build.ps1 

# Run the latest version of the container
.docker-run.ps1

$global:gitVersion = $null
