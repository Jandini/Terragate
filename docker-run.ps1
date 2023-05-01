
wsl docker network create kiboards-network
Start-Process "http://localhost:8088/swagger/index.html" -ErrorAction SilentlyContinue
wsl docker run --network kiboards-network -p 8088:80 -it -v data:/app/data -e ELASTICSEARCH_URI=http://kiboards-elastic:9200 -e TF_VAR_VRA_PASS=$([System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String($env:VRA_PASS))) -e ASPNETCORE_ENVIRONMENT=Development -e SERILOG__MINIMUMLEVEL=Debug --rm -t jandini/terragate:$("$(dotnet gitversion /showvariable SemVer)", "1.0.0" | Select-Object -First 1)
