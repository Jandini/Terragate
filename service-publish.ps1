# Publish solution in src directory as non self contained, linux-x64 runtime for docker container
dotnet publish -nologo --configuration Release --runtime linux-x64 --no-self-contained src

