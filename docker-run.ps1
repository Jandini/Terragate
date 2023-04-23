$ImageName = "jandini/terragate"
$ImageTag = $(dotnet gitversion /showvariable SemVer); if (!$ImageTag) { $ImageName = "1.0.0" }

Start-Process "http://localhost:8088/swagger/index.html" -ErrorAction SilentlyContinue
docker run --network kiboards-network -p 8088:80 -it -v ${PWD}\.data:/app/data -e ELASTICSEARCH_URI=http://kiboards-elastic:9200 -e ASPNETCORE_ENVIRONMENT=Development -e SERILOG__MINIMUMLEVEL=Debug -e TF_VAR_VRA_USER=$env:TF_VAR_VRA_USER -e TF_VAR_VRA_PASS=$env:TF_VAR_VRA_PASS --rm ${ImageName}:${ImageTag}
