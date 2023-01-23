.version.ps1
Write-Host "Opening http://localhost:8088/swagger/index.html"
Start-Process "chrome.exe" "http://localhost:8088/swagger/index.html" -ErrorAction SilentlyContinue
Write-Host "Executing docker run -p 8088:80 -it --rm $global:latestTag"
docker run -p 8088:80 -it --rm $global:latestTag

