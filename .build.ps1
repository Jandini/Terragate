# Use dotnet git version to retrive semantic version
Write-Host "Checking git version..."
$Version=$(dotnet gitversion /showvariable SemVer)
Write-Host "Current version is $Version"

# Create variables for image tags 
$Name="jandinis/terragate"
$Current="$($Name):$Version"
$Latest="$($Name):latest"

# Build docker container
Write-Host "Building docker image $Current"
docker build -t $Latest -t $Current .

# Run the latest version of the container
docker run -it --rm $Latest