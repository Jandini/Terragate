& .init.ps1

dotnet publish -nologo --configuration Release --runtime linux-x64 --no-self-contained -p:OutputType=Exe -p:DebugOutput=None src\Terragate.sln

if ($LASTEXITCODE -ne 0) {
    return
}

Start-Process "http://localhost:8088/swagger/index.html" -ErrorAction SilentlyContinue

docker build -t $global:currentTag .
docker run -p 8088:80 -it -e ASPNETCORE_ENVIRONMENT=Development -e TF_VAR_VRA_USER=$env:TF_VAR_VRA_USER -e TF_VAR_VRA_PASS=$env:TF_VAR_VRA_PASS --rm $global:currentTag

