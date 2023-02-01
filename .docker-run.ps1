& .git-version.ps1
Write-Host "Opening http://localhost:8088/swagger/index.html"
Start-Process "chrome.exe" "http://localhost:8088/swagger/index.html" -ErrorAction SilentlyContinue

Write-Host "Executing docker run..."
docker run -p 8088:80 -it -e ASPNETCORE_ENVIRONMENT=Development -e TF_VAR_VRA_USER=$env:TF_VAR_VRA_USER -e TF_VAR_VRA_PASS=$env:TF_VAR_VRA_PASS --rm $global:currentTag
