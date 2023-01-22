# Use dotnet git version to retrive semantic version
Write-Host "Getting git version..."
$version=$(dotnet gitversion /showvariable SemVer)
Write-Host "Current version is $version"

# Create variables for image tags 
$name="jandinis/terragate"
$current="$($name):$version"
$latest="$($name):latest"


# Build docker container
Write-Host "Building docker image $current..."
docker build -t $latest -t $current .
if ($LASTEXITCODE -eq 1) {
    # Check if docker services are running
    .docker.ps1
}
else {

    # Run the latest version of the container
    docker run -it --rm $latest
}