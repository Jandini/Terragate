# In some of my development machines I don't have docker desktop running all the time. 
# This script is resposible to spinup docker desktop if it is not running yet.

Write-Host "Checking docker service..."
if ((Get-Service -Name com.docker.service).Status -ne "Running") {
    Write-Warning "Docker service is not running."
    Write-Host "Starting com.docker.service..."
    Start-Service -Name com.docker.service

    Write-Host -NoNewline "Waiting docker service"
    while ((Get-Service -Name com.docker.service).Status -ne "Running") {
        Write-Host -NoNewline .
        Start-Sleep 1
    }
    Write-Host .
}

if (!(Get-Process 'com.docker.proxy' -ErrorAction SilentlyContinue)) {
    Write-Warning "Docker desktop is not running."
    Write-Host "Starting docker desktop..."
    Start-Process "C:\Program Files\Docker\Docker\Docker Desktop.exe" 
    Write-Host -NoNewline "Waiting for docker"
    while (!(Get-Process 'com.docker.proxy' -ErrorAction SilentlyContinue)) {
        Write-Host -NoNewline .
        Start-Sleep 1
    }
    Write-Host .
}