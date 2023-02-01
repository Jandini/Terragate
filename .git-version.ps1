# Use dotnet git version to retrive semantic version

if ($null -eq $global:gitVersion ) {
    Write-Host "Running GitVersion..."
    $global:gitVersion=$(dotnet gitversion /showvariable SemVer)
    Write-Host "Current version is $global:gitVersion"
}

# Create variables for image tags 
$imageName="jandinis/terragate"
$global:currentTag="$($imageName):$global:gitVersion"
$global:latestTag="$($imageName):latest"