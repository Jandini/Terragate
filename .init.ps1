# Use dotnet git version to retrive semantic version
$global:gitVersion=$(dotnet gitversion /showvariable SemVer)


& docker version *> $null
if ($LASTEXITCODE -ne 0) {
    # In some of my development machines I don't have docker desktop running all the time. 
    # This script is resposible for spinning up docker desktop if not already running.
    # Uncheck "Open Docker Dashboard at startup" in "General" docker desktop settings 
    # to prevent docker desktop window showing up.
    $service = { return (Get-Service -Name com.docker.service).Status }
    $status = & $service

    if ($status -ne "Running") {
        
        Write-Warning "Docker service is not running."
        Write-Host "Starting com.docker.service..."
        Start-Process "sc.exe" -ArgumentList "start com.docker.service" -Verb RunAs -WorkingDirectory $env:windir -Wait -WindowStyle Hidden
        
        Write-Host -NoNewline "Waiting docker service..."
        while ($status -ne "Running") {
            Write-Host -NoNewline .
            Start-Sleep 2
            $status = & $service
        }
        Write-Host .
    }

    if (!(Get-Process 'com.docker.proxy' -ErrorAction SilentlyContinue)) {
        Write-Warning "Docker desktop is not running."
        
        Write-Host "Starting docker desktop..."        
        & "${env:ProgramFiles}\Docker\Docker\Docker Desktop.exe" -Autostart
        
        Write-Host -NoNewline "Waiting for com.docker.proxy..."
        while (!(Get-Process 'com.docker.proxy' -ErrorAction SilentlyContinue)) {
            Write-Host -NoNewline .
            Start-Sleep 1
        }

        Write-Host .
        Write-Host -NoNewline "Waiting for docker..."

        $docker = {return Start-Process docker -WindowStyle Hidden -ArgumentList "container ls" -PassThru -Wait}
        $process = & $docker

        while ($process.ExitCode -ne 0) {
            Write-Host -NoNewline .
            Start-Sleep 2
            $process = & $docker
        }

        Write-Host .
    }    
}

Write-Host "Branch version $global:gitVersion"

# Create variables for image tags 
$imageName="jandini/terragate"
$global:currentTag="$($imageName):$global:gitVersion"
$global:latestTag="$($imageName):latest"
