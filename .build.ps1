# Publish application if not published and build docker image

.init.ps1

if (!(Test-Path ".\src\Terragate.Api\bin\Release\net7.0\linux-x64\publish")) {
    dotnet publish -nologo --configuration Release --runtime linux-x64 --no-self-contained -p:OutputType=Exe -p:DebugOutput=None src\Terragate.sln
}

docker build -t $global:currentTag .