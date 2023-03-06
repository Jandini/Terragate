# Do not display "Use 'docker scan' to run Snyk tests against images to find vulnerabilities and learn how to fix them"
$env:DOCKER_SCAN_SUGGEST="false"

# Use dotnet git version to retrive semantic version
$global:gitVersion=$(dotnet gitversion /showvariable SemVer)
Write-Host "Branch version $global:gitVersion"

# Create variables for image tags 
$imageName="jandini/terragate"
$global:currentTag="$($imageName):$global:gitVersion"
$global:latestTag="$($imageName):latest"

.\docker-start.ps1
