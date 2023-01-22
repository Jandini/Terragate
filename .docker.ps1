Write-Host "Checking docker service..."
if ((Get-Service -Name com.docker.service).Status -ne "Running") {
    Write-Warning "Docker service is not running."
    Write-Host "Starting com.docker.service..."
    Start-Service -Name com.docker.service
}

if (!(Get-Process 'com.docker.proxy' -ErrorAction SilentlyContinue)) {
    Write-Warning "Docker desktop is not running."
    Write-Host "Starting docker desktop..."
    Start-Process "C:\Program Files\Docker\Docker\Docker Desktop.exe" 
    Write-Warning "Wait for docker desktop to start and run the script again."
}