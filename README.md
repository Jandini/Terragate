# Terragate
[![Docker Image CI](https://github.com/Jandini/Terragate/actions/workflows/docker-image.yml/badge.svg)](https://github.com/Jandini/Terragate/actions/workflows/docker-image.yml)

Simple gateway API for terraform command line tool.



## Run 

Start the container with Terragate.

```sh
docker run -d -p 80:80 jandini/terragate
```





## Customize

You can create your own image and customize it to your needs with new `Dockerfile`.

```dockerfile
FROM jandini/terragate 
WORKDIR /app
EXPOSE 80
EXPOSE 443
# customize the image
ENTRYPOINT ["dotnet", "Terragate.Api.dll"]
```

You can customize the image...

* Add certificates 

  ```dockerfile
  # Add certificates from url or local path 
  ADD https://... /usr/local/share/ca-certificates/
  RUN update-ca-certificates
  ```

* Configure variables 

  ```dockerfile
  # Expose swagger interface
  ENV ASPNETCORE_ENVIRONMENT=Development
  ```

* Add your own `appsettings.json` configuration

  ```dockerfile
  WORKDIR /app
  ADD appsettings.json .
  ```

  ​

  ​

