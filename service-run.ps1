# The script is passing TF_VAR_VRA_USER and TF_VAR_VRA_PASS into the container from your local envrionment.

& .\service-init.ps1

dotnet publish -nologo --configuration Release --runtime linux-x64 --no-self-contained -p:OutputType=Exe -p:DebugOutput=None src\Terragate.sln
if ($LASTEXITCODE -ne 0) {
    return
}

Start-Process "http://localhost:8088/swagger/index.html" -ErrorAction SilentlyContinue

docker network create kiboards-network

docker build -t $global:currentTag .
docker run --network kiboards-network -p 8088:80 -it -v ${PWD}\.data:/app/data -e APPLICATION_NAME="Terragate (dev)" -e APPLICATION_VERSION=$global:currentTag -e ELASTICSEARCH_URI=http://kiboards-elastic:9200 -e ASPNETCORE_ENVIRONMENT=Development -e TF_VAR_VRA_USER=$env:TF_VAR_VRA_USER -e TF_VAR_VRA_PASS=$env:TF_VAR_VRA_PASS --rm $global:currentTag

 