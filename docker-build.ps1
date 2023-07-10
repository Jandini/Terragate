$version = "$(dotnet gitversion /showvariable SemVer)", "1.0.0" | Select-Object -First 1
Write-Host "Building jandini/terragate:$version..."
# Build docker image 
# Disabled build-arg. The values can be passed as runtime environment variables 
# wsl docker build --build-arg VRA_HOST=$env:VRA_HOST --build-arg VRA_TENANT=$env:VRA_TENANT --build-arg VRA_USER=$env:VRA_USER --build-arg VRA_PASS=$env:VRA_PASS -t jandini/terragate:${version} .
wsl docker build -t jandini/terragate:${version} .
# Inspect docker image 
wsl docker image inspect jandini/terragate:${version}

