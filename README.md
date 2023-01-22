# Terragate
Simple gateway API for terraform command line tool













# How To

All what you need to know to understand how Terragate was built.

## Execute command inside container

Start sample container in `-d` detached mode with `--rm` flat to remove container once is stopped.

```sh
docker run -d --rm -p 8000:80 --name aspnetcore_sample mcr.microsoft.com/dotnet/samples:aspnetapp
```

The new container id starts with `ba`.

```
ba04d41df2187e1d2efaf367f48be32934cc2b716f4e972bfd066e1106a084f9
```

Execute `ls` command from `ba`... container with `-it` interactive mode.

```sh
docker exec -it ba ls
```

Current folder content is displayed.

```sh
appsettings.Development.json  aspnetapp.pdb                                                       
appsettings.json              wwwroot
aspnetapp
```



## Download terraform inside container

By default base images like `mcr.microsoft.com/dotnet/aspnet:7.0 ` do not have tools like `wget` or `unzip`. You must install them in order to use them to download other prerequisites. 

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:7.0 
WORKDIR /app

# Install prerequisites
RUN apt-get update && \
    apt-get install -y wget unzip 

# Download and unpack terraform
RUN wget https://releases.hashicorp.com/terraform/1.3.7/terraform_1.3.7_linux_amd64.zip && \
    unzip terraform_1.3.7_linux_amd64.zip && \
    chmod +x terraform

ENTRYPOINT ./terraform
```

