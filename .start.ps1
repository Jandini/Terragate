.version.ps1
Write-Host "Executing docker run -it --rm $global:latestTag -p 8088:80"
docker run -it --rm $global:latestTag -p 8088:80
