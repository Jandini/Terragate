.version.ps1
Write-Host "Executing docker run -p 8088:80 -it --rm $global:latestTag"
docker run -p 8088:80 -it --rm $global:latestTag
