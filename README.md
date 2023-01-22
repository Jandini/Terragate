# Terragate
Simple gateway API for terraform command line tool.













# How to...

All you need to know about Terragate development and troubleshooting. 



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



**The certificate of 'releases.hashicorp.com' is not trusted. ** may be thrown when certificates are not updated. Make sure there certificates are provided and certificate repository is updated. 

```
 > [4/8] RUN wget https://releases.hashicorp.com/terraform/1.3.7/terraform_1.3.7_linux_amd64.zip:
 #7 0.364 --2023-01-22 15:58:36--  https://releases.hashicorp.com/terraform/1.3.7/terraform_1.3.7_linux_amd64.zip
 #7 0.372 Resolving releases.hashicorp.com (releases.hashicorp.com)... 108.138.85.30, 108.138.85.31, 108.138.85.65, ... 
 #7 0.660 Connecting to releases.hashicorp.com (releases.hashicorp.com)|108.138.85.30|:443... connected. 
 #7 1.187 ERROR: The certificate of 'releases.hashicorp.com' is not trusted. 
 #7 1.187 ERROR: The certificate of 'releases.hashicorp.com' doesn't have a known issuer. 
```

Ensure the certificates are provided and updated before download command is executed.

```dockerfile
COPY *.crt /usr/local/share/ca-certificates/
RUN update-ca-certificates
RUN wget https://releases.hashicorp.com/terraform/1.3.7/terraform_1.3.7_linux_amd64.zip 
```



## Terraform will not work without certificates

**Failed to query available provider packages** error is throw when terraform cannot reach out to the registry. 

```sh
│ Error: Failed to query available provider packages
│
│ Could not retrieve the list of available versions for provider vmware/vra7: could not connect to registry.terraform.io: Failed to request discovery document: Get
│ "https://registry.terraform.io/.well-known/terraform.json": x509: certificate signed by unknown authority
```

This indicate that your organization may be using something like Zscaler.  Add required certificate to `Dockerfile`.

``` dockerfile
COPY YOUR_CERTIFICATE.crt /usr/local/share/ca-certificates/YOUR_CERTIFICATE.crt
RUN update-ca-certificates
```

**Unable to get auth token** error indicate that you are still missing your organization certificates. 


```sh
│ Error: Error: Unable to get auth token: Post "https://cloud.xyz.com/identity/api/tokens": x509: certificate signed by unknown authority  
│
│   with provider["registry.terraform.io/vmware/vra7"],
│   on test.tf line 10, in provider "vra7":
│   10: provider "vra7" {
```

Terraform is not able to connect to the host specified in the provider. 

```json
provider "vra7" {
  username = ""
  password = ""
  tenant   = ""
  host     = "https://cloud.xyz.com"
}
```

Having to add multiple certificates can be resolved this way:

```dockerfile
COPY *.crt /usr/local/share/ca-certificates/
RUN update-ca-certificates
```

The copy will find all the files (recursively) from root of your repository and copy them to desired output. 

**IMPORTANT:**  The certificates must have CRT extension in `/usr/local/share/ca-certificates/`. Otherwise update-ca-certificates will not find them.



## How to export certificates

If terraform works on your machine, you can find and export required certificates. 

1. Open Manage user certificates or run `certlm.msc`.
2. Expand `Trusted Root Certification Authrorities` tree node.
3. Find your certificate and from context menu select `All Tasks > Export...`
4. Click `Next` in Welcome to the Certificate Export Wizard
5. Select `Base-64 encoded X.509 (.CER)` format. 
6. Save the file. Be aware that the extension will be changed to *.cer. The docker image is expecting *.crt. 

