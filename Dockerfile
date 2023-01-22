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




